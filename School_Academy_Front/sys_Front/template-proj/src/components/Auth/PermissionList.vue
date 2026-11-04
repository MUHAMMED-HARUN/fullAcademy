<template>
  <section class="space-y-4" dir="rtl" aria-label="قائمة الصلاحيات">
    <div
      v-if="props.permissions.length === 0"
      class="rounded-xl border border-dashed border-[var(--border-color)] p-6 text-center text-sm text-[var(--text-muted)]"
      role="status"
    >
      لا توجد صلاحيات متاحة لهذا الدور.
    </div>

    <div
      v-else
      class="max-h-[55vh] overflow-auto rounded-xl border border-[var(--border-color)]"
    >
      <table class="w-full text-start">
        <thead class="sticky top-0 bg-[var(--bg-body)]">
          <tr>
            <th class="px-3 py-2 text-start text-sm font-semibold">الصلاحية</th>
            <th class="px-3 py-2 text-start text-sm font-semibold">النوع</th>
            <th class="px-3 py-2 text-center text-sm font-semibold">مفعّلة</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-[var(--border-color)]">
          <tr v-for="permission in props.permissions" :key="permission.id">
            <td class="px-3 py-3 text-sm text-[var(--text-color)]">
              {{ permission.name }}
            </td>
            <td class="px-3 py-3 text-sm text-[var(--text-muted)]">
              {{ permission.type || "—" }}
            </td>
            <td class="px-3 py-3 text-center">
              <input
                type="checkbox"
                :checked="checkedNames.includes(permission.name)"
                :disabled="props.saving"
                :aria-label="`تفعيل ${permission.name}`"
                @change="
                  togglePermission(permission.name, $event.target.checked)
                "
              />
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <footer
      class="flex items-center justify-end gap-3 border-t border-[var(--border-color)] pt-4"
    >
      <span
        v-if="props.saving"
        class="text-sm text-[var(--text-muted)]"
        role="status"
      >
        جارٍ حفظ التغييرات...
      </span>
      <button
        type="button"
        class="btn btn-primary"
        :disabled="props.saving || props.permissions.length === 0"
        :aria-busy="props.saving"
        @click="emit('save', [...checkedNames])"
      >
        {{ props.saving ? "جارٍ الحفظ..." : "حفظ الصلاحيات" }}
      </button>
    </footer>
  </section>
</template>

<script setup>
import { ref, watch } from "vue";

const props = defineProps({
  permissions: { type: Array, default: () => [] },
  saving: { type: Boolean, default: false },
});

const emit = defineEmits(["save"]);
const checkedNames = ref([]);

watch(
  () => props.permissions,
  (permissions) => {
    checkedNames.value = permissions
      .filter((permission) => permission.isChecked)
      .map((permission) => permission.name);
  },
  { immediate: true, deep: true }
);

function togglePermission(name, isChecked) {
  checkedNames.value = isChecked
    ? [...new Set([...checkedNames.value, name])]
    : checkedNames.value.filter((permissionName) => permissionName !== name);
}
</script>
