<template>
  <div class="home-dashboard space-y-6">
    <!-- Welcome -->
    <section class="rounded-2xl border border-[var(--border-color)] bg-[var(--bg-card)] p-6 shadow-[var(--shadow-md)]">
      <div class="flex flex-col gap-2 md:flex-row md:items-center md:justify-between">
        <div>
          <h1 class="text-2xl font-bold text-[var(--text-color)]">
            مرحباً، {{ auth.userData?.name || "مستخدم" }}
          </h1>
          <p class="text-sm text-[var(--text-muted)]">
            {{ roleLabel }} — {{ today }}
          </p>
        </div>
        <div class="flex gap-2">
          <router-link
            v-if="canReadAcademicPlan"
            to="/academicPlan"
            class="rounded-lg bg-[var(--primary-color)] px-4 py-2 text-sm font-medium text-white hover:bg-[var(--primary-color-hover)]"
          >
            إدارة الخطط الأكاديمية
          </router-link>
        </div>
      </div>
    </section>

    <!-- Not authenticated -->
    <section
      v-if="!auth.isAuthenticated"
      class="rounded-2xl border border-dashed border-[var(--border-color)] bg-[var(--bg-card)] p-8 text-center text-[var(--text-muted)]"
    >
      يرجى تسجيل الدخول للوصول إلى لوحة المعلومات.
    </section>

    <!-- Academic Plans Cards -->
    <section v-else-if="canReadAcademicPlan">
      <div class="mb-3 flex items-center justify-between">
        <h2 class="text-lg font-semibold text-[var(--text-color)]">
          الخطط الأكاديمية
        </h2>
        <span class="text-sm text-[var(--text-muted)]">
          {{ academicPlanStore.totalCount }} خطة
        </span>
      </div>

      <!-- Loading -->
      <div
        v-if="academicPlanStore.loading"
        class="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3"
      >
        <div
          v-for="i in 6"
          :key="i"
          class="h-44 animate-pulse rounded-2xl bg-[var(--bg-light)]"
        ></div>
      </div>

      <!-- Empty -->
      <div
        v-else-if="!academicPlanStore.allAcademicPlans.length"
        class="rounded-2xl border border-dashed border-[var(--border-color)] bg-[var(--bg-card)] p-8 text-center text-[var(--text-muted)]"
      >
        لا توجد خطط أكاديمية حالياً.
      </div>

      <!-- Cards -->
      <div
        v-else
        class="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3"
      >
        <article
          v-for="plan in academicPlanStore.allAcademicPlans"
          :key="plan.academicPlanID"
          class="flex flex-col rounded-2xl border border-[var(--border-color)] bg-[var(--bg-card)] p-5 shadow-[var(--shadow-sm)] transition hover:shadow-[var(--shadow-md)]"
        >
          <div class="mb-3 flex items-start justify-between gap-2">
            <h3 class="text-base font-bold text-[var(--text-color)]">
              {{ plan.name }}
            </h3>
            <span
              class="rounded-full px-2 py-1 text-xs font-medium"
              :class="statusClass(plan.status)"
            >
              {{ statusLabel(plan.status) }}
            </span>
          </div>

          <dl class="space-y-1 text-sm text-[var(--text-muted)]">
            <div class="flex justify-between gap-2">
              <dt class="font-medium">المعهد:</dt>
              <dd class="text-[var(--text-color)]">
                {{ plan.providerInstituteName || "—" }}
              </dd>
            </div>
            <div class="flex justify-between gap-2">
              <dt class="font-medium">النطاق:</dt>
              <dd class="text-[var(--text-color)]">
                {{ plan.scopeName || "—" }}
              </dd>
            </div>
            <div class="flex justify-between gap-2">
              <dt class="font-medium">الإصدار:</dt>
              <dd class="text-[var(--text-color)]">{{ plan.version }}</dd>
            </div>
            <div class="flex justify-between gap-2">
              <dt class="font-medium">الصلاحية:</dt>
              <dd class="text-[var(--text-color)]">
                {{ plan.effectiveFrom }} — {{ plan.effectiveTo || "مستمر" }}
              </dd>
            </div>
            <div class="flex justify-between gap-2">
              <dt class="font-medium">المواد:</dt>
              <dd class="text-[var(--text-color)]">
                {{ plan.items?.length || 0 }}
              </dd>
            </div>
          </dl>
        </article>
      </div>
    </section>

    <!-- Non-academic-plan roles -->
    <section
      v-else
      class="rounded-2xl border border-dashed border-[var(--border-color)] bg-[var(--bg-card)] p-8 text-center text-[var(--text-muted)]"
    >
      لا توجد صلاحيات لعرض الخطط الأكاديمية. استخدم القائمة الجانبية للوصول إلى الميزات المتاحة.
    </section>
  </div>
</template>

<script setup>
import { computed, onMounted } from "vue";
import { useAuthStore } from "@/store/Auth/authStore";
import { useAcademicPlanStore } from "@/store/AcademicPlan/AcademicPlanStore";

const auth = useAuthStore();
const academicPlanStore = useAcademicPlanStore();

const canReadAcademicPlan = computed(() =>
  auth.hasPermission("Can_Read_AcademicPlan")
);

const roleLabel = computed(() => {
  const role = auth.UserRole;
  if (role === "Admin") return "مدير النظام";
  if (role === "Teacher") return "معلم";
  if (role === "Student") return "طالب";
  return role || "مستخدم";
});

const today = computed(() =>
  new Date().toLocaleDateString("ar-IQ", {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric",
  })
);

const statusLabel = (status) => {
  const map = { 1: "مسودة", 2: "نشطة", 3: "منتهية" };
  return map[status] || "—";
};

const statusClass = (status) => {
  if (status === 2) return "bg-emerald-100 text-emerald-700";
  if (status === 1) return "bg-amber-100 text-amber-700";
  return "bg-gray-100 text-gray-600";
};

onMounted(async () => {
  if (auth.isAuthenticated && canReadAcademicPlan.value) {
    await academicPlanStore.fetchAcademicPlans({ Page: 1, PageSize: 12 });
  }
});
</script>

<style scoped>
.home-dashboard {
  max-width: 1200px;
  margin: 0 auto;
}
</style>
