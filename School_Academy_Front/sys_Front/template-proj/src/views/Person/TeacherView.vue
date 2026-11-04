<template>
  <section class="space-y-5" dir="rtl" aria-label="إدارة المعلمين">
    <header class="flex flex-wrap items-center justify-between gap-3">
      <div>
        <h1 class="text-2xl font-bold text-[var(--primary-color)]">المعلمون</h1>
        <p class="text-sm text-[var(--text-muted)]">
          إدارة سجلات المعلمين والبحث ضمن النتائج.
        </p>
      </div>
      <button
        class="btn btn-primary"
        :disabled="TeacherStore.saving"
        @click="OnAddTeacher"
      >
        إضافة معلم
      </button>
    </header>

    <GenericFilterComp
      :fields="teacherFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <TeacherList
      :teachers="TeacherStore.teachers"
      :selected-ids="selectedTeacherIds"
      :loading="TeacherStore.loading"
      :error="TeacherStore.listError"
      @show-delete-teacher="OnDelete"
      @show-info-teacher="onShowInfo"
      @show-update-teacher="OnUpsertPerson"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
    />

    <GenericComboActions
      :actions="teacherActions"
      :selected-items="selectedTeachers"
      @action-click="handleComboAction"
    />

    <!-- 🔹 مكون التصفح -->
    <PaginationComp
      :current-page="TeacherStore.page"
      :page-size="TeacherStore.pageSize"
      :total-count="TeacherStore.totalCount"
      :total-pages="TeacherStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertTeacherComp
        :errors="TeacherStore.error"
        v-model="Model"
        :person-data="PersonStore.selectedPerson"
        :person-search-results="PersonStore.personSearchResults"
        :person-search-loading="PersonStore.personSearchLoading"
        :person-search-error="PersonStore.personSearchError"
        :saving="TeacherStore.saving"
        @submit="OnSubmitUpsert"
        @search-person="onSearchPerson"
        @select-person="onSelectPerson"
        @clear-person="onClearPerson"
        @person-query-change="onPersonQueryChange"
      />
    </PopapComp>

    <PopapComp v-if="showInfo" @close="onpopaClose">
      <TeacherCard :data-req="TeacherStore.selectedTeacher" />
    </PopapComp>

    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </section>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { usePersonStore } from "@/store/Person/usePersonStore";
import { useTeacherStore } from "@/store/Person/useTeacherStore";
import MessageComp from "@/components/general/messageComp.vue";
import TeacherCard from "@/components/Person/Teacher/TeacherCard.vue";
import TeacherList from "@/components/Person/Teacher/TeacherList.vue";
import UpsertTeacherComp from "@/components/Person/Teacher/UpsertTeachertComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";

export default {
  name: "TeacherView",
  components: {
    TeacherList,
    UpsertTeacherComp,
    TeacherCard,
    PopapComp,
    MessageComp,
    GenericFilterComp,
    PaginationComp,
    GenericComboActions,
  },
  data() {
    return {
      PersonStore: usePersonStore(),
      TeacherStore: useTeacherStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      selectedTeacherIds: [],
      teacherActions: [
        {
          key: "info",
          label: "معلومات",
          minSelection: 1,
          maxSelection: 1,
          class:
            "bg-[var(--secondary-color)] hover:bg-[var(--secondary-color-hover)]",
        },
        {
          key: "update",
          label: "تعديل",
          minSelection: 1,
          maxSelection: 1,
          class:
            "bg-[var(--primary-color)] hover:bg-[var(--primary-color-hover)]",
        },
        {
          key: "delete",
          label: "حذف",
          minSelection: 1,
          maxSelection: 1,
          class:
            "bg-[var(--danger-color)] hover:bg-[var(--danger-color-hover)]",
        },
      ],
      Model: {
        personID: "",
        teacherID: "",
      },
      teacherFilterSchema: [
        {
          key: "FirstName",
          label: "الاسم الأول",
          type: "text",
          operator: "=*",
        },
        {
          key: "LastName",
          label: "الكنية",
          type: "text",
          operator: "=*",
        },
        {
          key: "NationalNumber",
          label: "الرقم الوطني",
          type: "text",
          operator: "=*",
        },
      ],
    };
  },
  computed: {
    selectedTeachers() {
      return this.TeacherStore.teachers.filter((teacher) =>
        this.selectedTeacherIds.includes(teacher.teacherID)
      );
    },
  },
  methods: {
    async onPageChange(newPage) {
      this.selectedTeacherIds = [];
      await this.TeacherStore.setPage(newPage);
    },

    async onPageSizeChange(newSize) {
      this.selectedTeacherIds = [];
      await this.TeacherStore.setPageSize(newSize);
    },

    async onFilterSearch(payload) {
      this.selectedTeacherIds = [];
      await this.TeacherStore.fetchTeachers({
        Filter: payload.filter,
        OrderBy: payload.orderBy,
      });
    },

    async onFilterReset() {
      this.selectedTeacherIds = [];
      await this.TeacherStore.fetchTeachers({ Filter: "", OrderBy: "" });
    },

    OnAddTeacher() {
      this.selectedTeacherIds = [];
      this.Model = { teacherID: "", personID: "" };
      this.TeacherStore.selectedTeacher = null;
      this.TeacherStore.error = null;
      this.PersonStore.resetPersonCandidateSearch();
      this.showUpsert = true;
    },

    async OnUpsertPerson(teacher) {
      const teacherID = teacher?.teacherID;
      if (!teacherID) return;

      this.PersonStore.resetPersonCandidateSearch();
      const result = await this.TeacherStore.fetchTeacherBy({
        Param: teacherID,
        ParamType: 1,
      });
      if (!result.success || !result.data?.personID) {
        this.resultMessage = this.formatMessages(
          result.errors || "تعذر تحميل بيانات المعلم والشخص المرتبط به"
        );
        this.resultType = true;
        return;
      }

      this.Model = {
        teacherID: result.data.teacherID,
        personID: result.data.personID,
      };
      this.PersonStore.selectPersonCandidate(result.data);
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      const result = e.teacherID
        ? await this.TeacherStore.updateTeacher(e)
        : await this.TeacherStore.addTeacher(e);
      if (result.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
        this.onpopaClose();
      } else {
        this.resultMessage =
          this.formatMessages(result.errors) || "حدث خطأ أثناء الحفظ";
        this.resultType = true;
      }
    },

    async onSearchPerson(criteria) {
      await this.PersonStore.searchPersonCandidates(criteria);
    },

    onSelectPerson(person) {
      this.PersonStore.selectPersonCandidate(person);
    },

    onClearPerson() {
      this.PersonStore.clearSelectedPersonCandidate();
    },

    onPersonQueryChange() {
      this.PersonStore.resetPersonCandidateSearch();
    },

    formatMessages(value) {
      const flatten = (item) => {
        if (typeof item === "string") return [item];
        if (Array.isArray(item)) return item.flatMap(flatten);
        if (item && typeof item === "object") {
          return Object.values(item).flatMap(flatten);
        }
        return [];
      };
      return flatten(value).join("، ") || "حدث خطأ غير متوقع";
    },

    async OnDelete(e) {
      if (!e?.teacherID) return;
      const result = await this.TeacherStore.deleteTeacher(e.teacherID);
      this.resultMessage = result.success
        ? "تم حذف المعلم بنجاح"
        : this.formatMessages(result.errors);
      this.resultType = !result.success;
      if (result.success) {
        this.selectedTeacherIds = this.selectedTeacherIds.filter(
          (id) => id !== e.teacherID
        );
      }
    },

    async handleComboAction(actionKey, selectedItems) {
      const teacher = selectedItems[0];
      if (!teacher) return;

      switch (actionKey) {
        case "info":
          await this.onShowInfo(teacher);
          break;
        case "update":
          await this.OnUpsertPerson(teacher);
          break;
        case "delete":
          await this.OnDelete(teacher);
          break;
      }
    },

    onToggleSelectAll(isChecked) {
      this.selectedTeacherIds = isChecked
        ? this.TeacherStore.teachers.map((teacher) => teacher.teacherID)
        : [];
    },

    onToggleSelectRow(teacher) {
      const id = teacher.teacherID;
      const index = this.selectedTeacherIds.indexOf(id);
      if (index > -1) this.selectedTeacherIds.splice(index, 1);
      else this.selectedTeacherIds.push(id);
    },

    async onShowInfo(e) {
      this.TeacherStore.selectedTeacher = e;
      this.showInfo = true;
    },

    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.TeacherStore.selectedTeacher = null;
      this.Model = { teacherID: "", personID: "" };
      this.PersonStore.resetPersonCandidateSearch();
    },
  },

  mounted() {
    this.TeacherStore.fetchTeachers();
  },
};
</script>
