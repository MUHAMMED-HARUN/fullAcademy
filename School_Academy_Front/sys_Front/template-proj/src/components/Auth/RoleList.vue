<template>
  <section class="space-y-5" dir="rtl" aria-label="إدارة الأدوار والصلاحيات">
    <header>
      <h1 class="text-2xl font-bold text-[var(--primary-color)]">الأدوار</h1>
      <p class="mt-1 text-sm text-[var(--text-muted)]">
        راجع الصلاحيات الممنوحة لكل دور.
      </p>
    </header>

    <div
      v-if="loadingRoles"
      class="rounded-xl border border-[var(--border-color)] bg-[var(--bg-card)] p-6 text-center text-[var(--text-muted)]"
      role="status"
    >
      جارٍ تحميل الأدوار...
    </div>
    <div v-else-if="rolesError" class="alert-error" role="alert">
      {{ rolesError }}
    </div>
    <div
      v-else-if="roles.length === 0"
      class="rounded-xl border border-dashed border-[var(--border-color)] bg-[var(--bg-card)] p-8 text-center text-[var(--text-muted)]"
      role="status"
    >
      لا توجد أدوار متاحة.
    </div>
    <div
      v-else
      class="overflow-hidden rounded-xl border border-[var(--border-color)] bg-[var(--bg-card)]"
    >
      <div class="overflow-x-auto">
        <table class="min-w-[32rem] w-full text-start">
          <thead
            class="border-b border-[var(--border-color)] bg-[var(--bg-body)]"
          >
            <tr>
              <th class="px-4 py-3 text-start text-sm font-semibold">#</th>
              <th class="px-4 py-3 text-start text-sm font-semibold">الدور</th>
              <th class="px-4 py-3 text-end text-sm font-semibold">الإجراء</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-[var(--border-color)]">
            <tr v-for="(role, index) in roles" :key="role.id">
              <td class="px-4 py-3 text-sm text-[var(--text-muted)]">
                {{ index + 1 }}
              </td>
              <td
                class="px-4 py-3 text-sm font-medium text-[var(--text-color)]"
              >
                {{ role.name }}
              </td>
              <td class="px-4 py-3 text-end">
                <button
                  type="button"
                  class="btn btn-sm btn-outline-primary"
                  :disabled="isAdminRole(role)"
                  :aria-label="`تعديل صلاحيات دور ${role.name}`"
                  @click="openPermissions(role)"
                >
                  {{
                    isAdminRole(role)
                      ? "صلاحيات الإدارة كاملة"
                      : "تعديل الصلاحيات"
                  }}
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <PopapComp v-if="showPermissions" @close="closePermissions">
      <section class="space-y-4" dir="rtl" aria-label="صلاحيات الدور">
        <header>
          <h2 class="text-lg font-bold text-[var(--primary-color)]">
            صلاحيات دور {{ selectedRole?.name }}
          </h2>
          <p class="text-sm text-[var(--text-muted)]">
            حدّد الصلاحيات التي يحتاجها مستخدمو هذا الدور.
          </p>
        </header>

        <p v-if="permissionsError" class="alert-error" role="alert">
          {{ permissionsError }}
        </p>
        <p
          v-if="loadingPermissions"
          class="py-5 text-center text-[var(--text-muted)]"
          role="status"
        >
          جارٍ تحميل الصلاحيات...
        </p>
        <PermissionList
          v-else
          :permissions="permissions"
          :saving="savingPermissions"
          @save="savePermissions"
        />
      </section>
    </PopapComp>
  </section>
</template>

<script setup>
import { onMounted, ref } from "vue";
import PopapComp from "@/components/general/popapComp.vue";
import PermissionList from "@/components/Auth/PermissionList.vue";
import { useAuthStore } from "@/store/Auth/authStore";
import { useToast } from "@/Generic/useToast";

const authStore = useAuthStore();
const toast = useToast();
const roles = ref([]);
const permissions = ref([]);
const selectedRole = ref(null);
const originalPermissionNames = ref(new Set());
const loadingRoles = ref(false);
const loadingPermissions = ref(false);
const savingPermissions = ref(false);
const rolesError = ref("");
const permissionsError = ref("");
const showPermissions = ref(false);

const flattenMessages = (value) => {
  if (typeof value === "string") return [value];
  if (Array.isArray(value)) return value.flatMap(flattenMessages);
  if (value && typeof value === "object") {
    return Object.values(value).flatMap(flattenMessages);
  }
  return [];
};
const isAdminRole = (role) => role?.name?.toLowerCase() === "admin";

const formatError = (value) =>
  flattenMessages(value).join("، ") || "تعذر تنفيذ العملية";

async function loadRoles() {
  loadingRoles.value = true;
  rolesError.value = "";
  try {
    const result = await authStore.getRoles();
    if (result.success) roles.value = result.data || [];
    else rolesError.value = formatError(result.errors);
  } finally {
    loadingRoles.value = false;
  }
}

async function openPermissions(role) {
  selectedRole.value = role;
  permissions.value = [];
  permissionsError.value = "";
  originalPermissionNames.value = new Set();
  showPermissions.value = true;
  loadingPermissions.value = true;

  try {
    const [roleResult, allResult] = await Promise.all([
      authStore.getAllRolePermissions(role.id),
      authStore.getPermissions(),
    ]);
    if (!roleResult.success || !allResult.success) {
      permissionsError.value = formatError(
        roleResult.errors || allResult.errors
      );
      return;
    }

    const assigned = new Set(
      (roleResult.data || []).map((item) => item.permissionName)
    );
    originalPermissionNames.value = assigned;
    permissions.value = (allResult.data || []).map((permission) => ({
      ...permission,
      isChecked: assigned.has(permission.name),
    }));
  } finally {
    loadingPermissions.value = false;
  }
}

async function savePermissions(checkedNames) {
  if (!selectedRole.value || isAdminRole(selectedRole.value)) return;

  const checked = new Set(checkedNames);
  const changes = permissions.value
    .filter(
      (permission) =>
        checked.has(permission.name) !==
        originalPermissionNames.value.has(permission.name)
    )
    .map((permission) => ({
      roleId: selectedRole.value.id,
      permissionName: permission.name,
      isDelete: !checked.has(permission.name),
    }));

  if (changes.length === 0) {
    toast.info("لا توجد تغييرات لحفظها");
    return;
  }

  savingPermissions.value = true;
  permissionsError.value = "";
  try {
    const result = await authStore.UpdateRolePermissions({ changes });
    if (!result.success) {
      permissionsError.value = formatError(result.errors);
      toast.error("فشل تحديث الصلاحيات", permissionsError.value);
      return;
    }

    toast.success("تم تحديث صلاحيات الدور");
    closePermissions();
  } finally {
    savingPermissions.value = false;
  }
}

function closePermissions() {
  showPermissions.value = false;
  selectedRole.value = null;
  permissions.value = [];
  originalPermissionNames.value = new Set();
}

onMounted(loadRoles);
</script>
