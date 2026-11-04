<template>
  <div>
    <!-- 🔹 فلتر البحث الشامل -->
    <GenericFilterComp
      :fields="personFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <GenericComboActions
      :actions="visibleActions"
      :selected-items="selectedPersons"
      @action-click="handleComboAction"
    />

    <PersonList
      :persons="PersonStore.persons"
      :selected-ids="selectedIds"
      :loading="PersonStore.loading || lookupsLoading"
      :error="PersonStore.listError"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @show-update-person="OnUpsertPerson"
      @show-delete-person="OnDeletePerson"
      @show-info-person="onShowInfoPerson"
    />

    <!-- 🔹 مكون التصفح -->
    <PaginationComp
      :current-page="PersonStore.page"
      :page-size="PersonStore.pageSize"
      :total-count="PersonStore.totalCount"
      :total-pages="PersonStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertPersonAddress
        :countries="CountryStore.countries"
        :countries-loading="CountryStore.loading"
        :countries-error="CountryStore.error"
        :birth-city-data="birthCityData"
        :birth-city-results="CityStore.cityLookup('personBirthPlace').results"
        :birth-city-search-loading="
          CityStore.cityLookup('personBirthPlace').loading
        "
        :birth-city-search-error="
          CityStore.cityLookup('personBirthPlace').error
        "
        :address-city-results="
          CityStore.cityLookup('personAddressCity').results
        "
        :address-city-search-loading="
          CityStore.cityLookup('personAddressCity').loading
        "
        :address-city-search-error="
          CityStore.cityLookup('personAddressCity').error
        "
        :address-district-results="DistrictStore.districtSearchResults"
        :address-district-search-loading="DistrictStore.districtSearchLoading"
        :address-district-search-error="DistrictStore.districtSearchError"
        :address-neighborhood-results="
          NeighborhoodStore.neighborhoodSearchResults
        "
        :address-neighborhood-search-loading="
          NeighborhoodStore.neighborhoodSearchLoading
        "
        :address-neighborhood-search-error="
          NeighborhoodStore.neighborhoodSearchError
        "
        :errors="PersonStore.error"
        :loading="PersonStore.loading || addressHydrationLoading"
        :address="AddressStore.selectedaddress"
        @on-submit="OnSubmitUpsert"
        @search-birth-city="onSearchBirthCity"
        @clear-birth-city-search="
          CityStore.clearCitySearchFor('personBirthPlace')
        "
        @search-address-city="onSearchAddressCity"
        @clear-address-city-search="
          CityStore.clearCitySearchFor('personAddressCity')
        "
        @search-address-district="onSearchAddressDistrict"
        @clear-address-district-search="DistrictStore.clearDistrictSearch"
        @search-address-neighborhood="onSearchAddressNeighborhood"
        @clear-address-neighborhood-search="
          NeighborhoodStore.clearNeighborhoodSearch
        "
      />
    </PopapComp>

    <PopapComp v-if="showInfo" @close="onpopaClose">
      <PersonCard :data-req="PersonStore.selectedPerson" />
    </PopapComp>
    <PopapComp v-if="showUpsertStudent" @close="onpopaCloseStudent">
      <UpsertStudentComp
        v-model="modelValue"
        :person-data="PersonStore.selectedPerson"
        :person-locked="true"
        :saving="StudentStore.saving"
        :errors="StudentStore.error"
        @submit="OnSubmitStudent"
      />
    </PopapComp>
    <PopapComp v-if="showUpsertTeacher" @close="onpopaCloseTeacher">
      <UpsertTeacherComp
        v-model="modelValue"
        :person-data="PersonStore.selectedPerson"
        :person-locked="true"
        :saving="TeacherStore.saving"
        :errors="TeacherStore.error"
        @submit="OnSubmitTeacher"
      />
    </PopapComp>
    <PopapComp v-if="showUpsertUser" @close="onpopaCloseUser">
      <UpsertUserComp
        :errors="AuthStore.error"
        :roles="roles"
        :entity-id="PersonStore.selectedPerson?.id"
        :user-type="1"
        :saving="AuthStore.saving"
        @on-submit="submitUser"
      />
    </PopapComp>

    <button
      class="btn btn-primary"
      :disabled="lookupsLoading"
      @click="onAddPerson"
    >
      إضافة شخص
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { usePersonStore } from "@/store/Person/usePersonStore";
import { useAddressStore } from "@/store/Address/useAddreessStore";
import { useCountryStore } from "@/store/Address/useCountryStore";
import { useCityStore } from "@/store/Address/useCityStore";
import { useDistrictStore } from "@/store/Address/useDistrictStore";
import { useNeighborhoodStore } from "@/store/Address/useNeighborhoodStore";
import { useAuthStore } from "@/store/Auth/authStore";
import { useStudentStore } from "@/store/Person/useStudentStore";
import { useTeacherStore } from "@/store/Person/useTeacherStore";
import MessageComp from "@/components/general/messageComp.vue";
import PersonList from "@/components/Person/Person/PersonList.vue";
import UpsertPersonAddress from "@/components/Person/Person/upsertPersonAddress.vue";
import PersonCard from "@/components/Person/Person/PersonCard.vue";
import UpsertStudentComp from "@/components/Person/Student/UpsertStudentComp.vue";
import UpsertTeacherComp from "@/components/Person/Teacher/UpsertTeachertComp.vue";
import UpsertUserComp from "@/components/Auth/UpsertUserComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";

export default {
  name: "PersonView",
  components: {
    PersonList,
    UpsertPersonAddress,
    PopapComp,
    MessageComp,
    PersonCard,
    UpsertStudentComp,
    UpsertTeacherComp,
    UpsertUserComp,
    GenericFilterComp,
    PaginationComp,
    GenericComboActions,
  },
  data() {
    return {
      PersonStore: usePersonStore(),
      StudentStore: useStudentStore(),
      TeacherStore: useTeacherStore(),
      AddressStore: useAddressStore(),
      CountryStore: useCountryStore(),
      CityStore: useCityStore(),
      DistrictStore: useDistrictStore(),
      NeighborhoodStore: useNeighborhoodStore(),
      AuthStore: useAuthStore(),
      roles: [],
      showUpsert: false,
      showInfo: false,
      showUpsertStudent: false,
      showUpsertTeacher: false,
      showUpsertUser: false,
      lookupsLoading: true,
      addressHydrationLoading: false,
      birthCityData: null,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      modelValue: {
        personID: "",
        studentID: "",
        teacherID: "",
      },
      actions: [
        { key: "info", label: "معلومات", minSelection: 1, maxSelection: 1 },
        { key: "update", label: "تحديث", minSelection: 1, maxSelection: 1 },
        {
          key: "delete",
          label: "حذف",
          minSelection: 1,
          class: "bg-red-500 hover:bg-red-600",
        },
        {
          key: "setAsStudent",
          label: "تعيين كطالب",
          minSelection: 1,
          class: "bg-brand-500 hover:bg-brand-600",
        },
        {
          key: "setAsTeacher",
          label: "تعيين كمعلم",
          minSelection: 1,
          class: "bg-brand-500 hover:bg-brand-600",
        },
        {
          key: "setAsUser",
          label: "تعيين كمستخدم",
          minSelection: 1,
          class: "bg-brand-500 hover:bg-brand-600",
        },
      ],
      userForm: {
        userName: "",
        email: "",
        password: "",
        roleID: "",
        userType: 1,
      },
      personFilterSchema: [
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
        {
          key: "Gendor",
          label: "الجنس",
          type: "select",
          options: [
            { label: "ذكر", value: true },
            { label: "أنثى", value: false },
          ],
        },
      ],
    };
  },
  computed: {
    visibleActions() {
      return this.actions.filter(
        (action) =>
          action.key !== "setAsUser" ||
          this.AuthStore.hasPermission("Can_Register_User")
      );
    },
    selectedPersons() {
      return this.PersonStore.persons.filter((p) =>
        this.selectedIds.includes(p.id)
      );
    },
  },
  methods: {
    onAddPerson() {
      this.PersonStore.selectedPerson = null;
      this.AddressStore.selectedaddress = null;
      this.birthCityData = null;
      this.CityStore.clearCitySearchFor("personBirthPlace");
      this.CityStore.clearCitySearchFor("personAddressCity");
      this.CityStore.selectedCity = null;
      this.DistrictStore.clearDistrictSearch();
      this.DistrictStore.selecteddistricts = null;
      this.NeighborhoodStore.clearNeighborhoodSearch();
      this.NeighborhoodStore.selectedNeighborhood = null;
      this.CountryStore.selectedCountry = null;
      this.showUpsert = true;
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

    async onPageChange(newPage) {
      await this.PersonStore.setPage(newPage);
    },

    async onPageSizeChange(newSize) {
      await this.PersonStore.setPageSize(newSize);
    },

    async onFilterSearch(payload) {
      await this.PersonStore.fetchPersons({
        Filter: payload.filter,
        OrderBy: payload.orderBy,
      });
    },

    async onFilterReset() {
      await this.PersonStore.fetchPersons();
    },

    async OnUpsertPerson(e) {
      if (e) {
        this.addressHydrationLoading = true;
        this.PersonStore.selectedPerson = null;
        this.AddressStore.selectedaddress = null;
        this.birthCityData = null;
        this.CityStore.clearCitySearchFor("personBirthPlace");
        this.CityStore.clearCitySearchFor("personAddressCity");
        this.CityStore.selectedCity = null;
        this.DistrictStore.clearDistrictSearch();
        this.DistrictStore.selecteddistricts = null;
        this.NeighborhoodStore.clearNeighborhoodSearch();
        this.NeighborhoodStore.selectedNeighborhood = null;

        try {
          const result = await this.PersonStore.fetchPersonForUpsert(e.id);
          if (!result.success) {
            this.resultMessage = this.formatMessages(
              result.errors || "تعذر جلب بيانات الشخص للتعديل"
            );
            this.resultType = true;
            return;
          }

          let address = result.data.address;
          if (address?.neighborhoodID) {
            const hierarchy =
              await this.NeighborhoodStore.fetchNeighborhoodById(
                address.neighborhoodID
              );
            if (!hierarchy.success || !hierarchy.data) {
              this.resultMessage = this.formatMessages(
                hierarchy.errors || "تعذر تهيئة تسلسل العنوان للتعديل"
              );
              this.resultType = true;
              return;
            }
            address = {
              ...address,
              countryName: hierarchy.data.countryName,
              cityName: hierarchy.data.cityName,
              districtName: hierarchy.data.districtName,
              neighborhoodName: hierarchy.data.name,
            };
          } else if (address?.districtID) {
            const district = await this.DistrictStore.fetchDistrictById(
              address.districtID
            );
            if (district.success && district.data) {
              address = {
                ...address,
                countryName: district.data.countryName,
                cityName: district.data.cityName,
                districtName: district.data.name,
              };
            }
          } else if (address?.cityID) {
            const city = await this.CityStore.fetchCityById(address.cityID);
            if (city.success && city.data) {
              address = {
                ...address,
                countryName: city.data.countryName,
                cityName: city.data.name,
              };
            }
          }

          this.AddressStore.selectedaddress = address;

          if (result.data.placeOfBirthID) {
            const birthCity = await this.CityStore.fetchCityById(
              result.data.placeOfBirthID
            );
            if (!birthCity.success || !birthCity.data) {
              this.resultMessage = this.formatMessages(
                birthCity.errors || "تعذر تهيئة مدينة الولادة للتعديل"
              );
              this.resultType = true;
              return;
            }
            this.birthCityData = birthCity.data;
          }
        } finally {
          this.addressHydrationLoading = false;
        }
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = false;
      if (e.id) {
        resVal = await this.PersonStore.updatePerson(e);
      } else {
        resVal = await this.PersonStore.addPerson(e);
        this.CountryStore.selectedCountry = null;
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
        this.PersonStore.selectedPerson = null;
        this.AddressStore.selectedaddress = null;
      } else {
        this.resultMessage = this.formatMessages(
          resVal.errors || "حدث خطأ أثناء الحفظ"
        );
        this.resultType = true;
      }
    },

    async OnDeletePerson(e) {
      const result = await this.PersonStore.DeletePerson(e.id);
      if (result.success) {
        this.resultMessage = "تم حذف الشخص بنجاح";
        this.resultType = false;
      } else {
        this.resultMessage = this.formatMessages(result.errors);
        this.resultType = true;
      }
    },

    async onShowInfoPerson(e) {
      this.PersonStore.selectedPerson = this.PersonStore.persons.find(
        (p) => p.id === e.id
      );
      this.showInfo = true;
    },

    SetAsStudent(person) {
      this.PersonStore.resetPersonCandidateSearch();
      this.PersonStore.selectedPerson = person;
      this.StudentStore.error = null;
      this.modelValue = { studentID: "", personID: person.id };
      this.showUpsertStudent = true;
    },
    SetAsTeacher(person) {
      this.PersonStore.resetPersonCandidateSearch();
      this.PersonStore.selectedPerson = person;
      this.TeacherStore.error = null;
      this.modelValue = { teacherID: "", personID: person.id };
      this.showUpsertTeacher = true;
    },
    async SetAsUser(person) {
      if (!this.AuthStore.hasPermission("Can_Register_User")) return;
      this.PersonStore.selectedPerson = person;
      this.userForm = {
        userName: "",
        email: "",
        password: "",
        roleID: "",
        userType: 1,
      };
      const res = await this.AuthStore.getRoles();
      if (res && res.success) this.roles = res.data || [];
      this.showUpsertUser = true;
    },
    async submitUser(e) {
      const person = this.PersonStore.selectedPerson;
      const res = await this.AuthStore.register({
        userName: e.userName,
        email: e.email,
        password: e.password,
        confirmPassword: e.confirmPassword,
        roleID: e.roleID,
        userType: e.userType || 1,
        entityID: e.entityID || person.id,
      });

      if (res.success) {
        this.resultMessage = "تم تعيينه كمستخدم";
        this.resultType = false;
        this.showUpsertUser = false;
      } else {
        this.resultMessage =
          (res.errors && (res.errors.general || res.errors.errors)) ||
          "فشل التعيين";
        this.resultType = true;
      }
    },
    async OnSubmitStudent(e) {
      const res = await this.StudentStore.addStudent({ personID: e.personID });
      if (res.success) {
        this.resultMessage = "تم تعيينه كطالب";
        this.resultType = false;
        this.onpopaCloseStudent();
      } else {
        this.resultMessage =
          (res.errors && res.errors.general) || "فشل التعيين";
        this.resultType = true;
      }
    },
    async OnSubmitTeacher(e) {
      const res = await this.TeacherStore.addTeacher({ personID: e.personID });
      if (res.success) {
        this.resultMessage = "تم تعيينه كمعلم";
        this.resultType = false;
        this.onpopaCloseTeacher();
      } else {
        this.resultMessage =
          (res.errors && res.errors.general) || "فشل التعيين";
        this.resultType = true;
      }
    },
    onpopaCloseStudent() {
      this.showUpsertStudent = false;
      this.PersonStore.selectedPerson = null;
      this.PersonStore.resetPersonCandidateSearch();
      this.modelValue = { studentID: "", teacherID: "", personID: "" };
    },
    onpopaCloseTeacher() {
      this.showUpsertTeacher = false;
      this.PersonStore.selectedPerson = null;
      this.PersonStore.resetPersonCandidateSearch();
      this.modelValue = { studentID: "", teacherID: "", personID: "" };
    },
    onpopaCloseUser() {
      this.showUpsertUser = false;
      this.PersonStore.selectedPerson = null;
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          await this.onShowInfoPerson(singleItem);
          break;
        case "update":
          await this.OnUpsertPerson(singleItem);
          break;
        case "delete":
          await this.OnDeletePerson(singleItem);
          this.selectedIds = [];
          break;
        case "setAsStudent":
          await this.SetAsStudent(singleItem);
          break;
        case "setAsTeacher":
          await this.SetAsTeacher(singleItem);
          break;
        case "setAsUser":
          this.SetAsUser(singleItem);
          break;
      }
    },
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.PersonStore.persons.map((p) => p.id)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.id);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.id);
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.PersonStore.selectedPerson = null;
      this.AddressStore.selectedaddress = null;
      this.birthCityData = null;
      this.CityStore.clearCitySearchFor("personBirthPlace");
      this.CityStore.clearCitySearchFor("personAddressCity");
      this.CityStore.selectedCity = null;
      this.DistrictStore.clearDistrictSearch();
      this.DistrictStore.selecteddistricts = null;
      this.NeighborhoodStore.clearNeighborhoodSearch();
      this.NeighborhoodStore.selectedNeighborhood = null;
      this.CountryStore.selectedCountry = null;
    },

    async onSearchBirthCity(criteria) {
      await this.CityStore.searchCitiesFor(criteria, "personBirthPlace");
    },
    async onSearchAddressCity(criteria) {
      await this.CityStore.searchCitiesFor(criteria, "personAddressCity");
    },
    async onSearchAddressDistrict(criteria) {
      await this.DistrictStore.searchDistricts(criteria);
    },
    async onSearchAddressNeighborhood(criteria) {
      await this.NeighborhoodStore.searchNeighborhoods(criteria);
    },
  },

  async mounted() {
    try {
      await Promise.all([this.CountryStore.fetchCountries()]);
    } finally {
      this.lookupsLoading = false;
    }
    await this.PersonStore.fetchPersons();
  },
};
</script>
