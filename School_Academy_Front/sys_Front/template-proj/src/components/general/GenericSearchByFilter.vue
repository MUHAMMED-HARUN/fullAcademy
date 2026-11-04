<template>
  <div class="relative w-full space-y-2" dir="rtl">
    <div class="flex flex-col gap-2 sm:flex-row sm:items-end">
      <div class="w-full sm:max-w-40">
        <label
          :for="`${listboxId}-type`"
          class="mb-1 block text-sm font-medium text-[var(--text-color)]"
        >
          نوع البحث
        </label>
        <select
          :id="`${listboxId}-type`"
          v-model="filterType"
          class="form-select"
          :disabled="loading"
        >
          <option
            v-for="type in filterTypes"
            :key="type.value"
            :value="type.value"
          >
            {{ type.label }}
          </option>
        </select>
      </div>

      <div class="flex min-w-0 flex-1 gap-2">
        <div class="relative min-w-0 flex-1">
          <label
            :for="`${listboxId}-query`"
            class="mb-1 block text-sm font-medium text-[var(--text-color)]"
          >
            قيمة البحث
          </label>
          <input
            :id="`${listboxId}-query`"
            v-model="query"
            class="form-control"
            type="search"
            :placeholder="placeholder"
            role="combobox"
            aria-autocomplete="list"
            aria-haspopup="listbox"
            :aria-expanded="showPopup"
            :aria-controls="listboxId"
            :aria-activedescendant="activeOptionId || undefined"
            :disabled="loading"
            @keydown="onInputKeydown"
          />
        </div>

        <button
          type="button"
          class="btn btn-primary min-w-24 self-end"
          :disabled="loading || (!query.trim() && !hasInitialSuggestions)"
          :aria-busy="loading"
          @click="onSearch"
        >
          <span
            v-if="loading"
            class="inline-block h-4 w-4 animate-spin rounded-full border-2 border-white/40 border-t-white"
            aria-hidden="true"
          />
          {{ loading ? "جاري البحث" : "بحث" }}
        </button>

        <button
          v-if="canToggleResults"
          type="button"
          class="btn btn-outline-secondary min-w-24 self-end"
          :aria-expanded="showPopup"
          @click="toggleResults"
        >
          {{ showPopup ? "إخفاء النتائج" : "عرض النتائج" }}
        </button>
      </div>
    </div>

    <div
      v-if="selectedLabel"
      class="flex items-center justify-between gap-3 rounded-lg border border-[var(--primary-color-light)] bg-[var(--primary-color-light)] px-3 py-2 text-sm"
      aria-live="polite"
    >
      <span class="min-w-0 truncate font-medium text-[var(--primary-color)]">
        {{ selectedLabel }}
      </span>
      <button
        type="button"
        class="btn btn-sm btn-outline-danger shrink-0"
        aria-label="مسح الاختيار"
        @click="clearSelection"
      >
        مسح
      </button>
    </div>

    <p v-if="errorText" class="alert-error" role="alert" aria-live="assertive">
      {{ errorText }}
    </p>

    <p
      v-else-if="showEmptyState"
      class="rounded-lg border border-dashed border-[var(--border-color)] bg-[var(--bg-body)] p-4 text-center text-sm text-[var(--text-muted)]"
      role="status"
      aria-live="polite"
    >
      {{ emptyMessage }}
    </p>

    <div
      v-if="showPopup && results.length"
      :id="listboxId"
      class="absolute inset-x-0 top-full z-50 mt-1 max-h-64 overflow-y-auto rounded-xl border border-[var(--border-color)] bg-[var(--bg-card)] p-1 shadow-lg"
      role="listbox"
      :aria-label="`نتائج ${placeholder}`"
    >
      <button
        v-for="(item, index) in results"
        :id="`${listboxId}-option-${index}`"
        :key="item[idField] ?? index"
        type="button"
        class="block w-full rounded-lg px-3 py-2 text-start text-sm text-[var(--text-color)] transition-colors hover:bg-[var(--primary-color-light)] focus-visible:outline focus-visible:outline-2 focus-visible:outline-[var(--primary-color)]"
        :class="
          activeIndex === index
            ? 'bg-[var(--primary-color-light)] text-[var(--primary-color)]'
            : ''
        "
        role="option"
        :aria-selected="
          String(selectedItem?.[idField]) === String(item[idField])
        "
        @mouseenter="activeIndex = index"
        @click="selectItem(item)"
      >
        {{ item[nameField] ?? item[idField] }}
      </button>
    </div>
  </div>
</template>

<script setup>
import { computed, nextTick, ref, useId, watch } from "vue";

const props = defineProps({
  filterTypes: {
    type: Array,
    default: () => [
      { value: 0, label: "ID" },
      { value: 1, label: "الاسم" },
    ],
  },
  modelValue: { type: [String, Number], default: "" },
  nameValue: { type: String, default: "" },
  results: { type: Array, default: () => [] },
  idField: { type: String, default: "id" },
  nameField: { type: String, default: "name" },
  placeholder: { type: String, default: "أدخل قيمة البحث" },
  loading: { type: Boolean, default: false },
  error: { type: [String, Array, Object], default: null },
  emptyMessage: { type: String, default: "لا توجد نتائج مطابقة" },
  initialSuggestions: { type: Boolean, default: false },
  suggestionKey: { type: [String, Number], default: "" },
});

const emit = defineEmits([
  "update:modelValue",
  "update:nameValue",
  "search",
  "select",
  "clear",
  "query-change",
  "no-results",
]);

const listboxId = `lookup-${useId()}`;
const filterType = ref(props.filterTypes[0]?.value ?? 0);
const query = ref("");
const showPopup = ref(false);
const hasSearched = ref(false);
const hasExplicitSearch = ref(false);
const activeIndex = ref(-1);
const selectedItem = ref(null);
const suppressQueryWatch = ref(false);
const responseHandled = ref(false);
const currentSearchIsInitial = ref(false);

const results = computed(() =>
  Array.isArray(props.results) ? props.results : []
);
const hasSelectedId = computed(
  () =>
    props.modelValue !== "" &&
    props.modelValue !== null &&
    props.modelValue !== undefined
);
const selectedLabel = computed(() => {
  const itemLabel = selectedItem.value?.[props.nameField];
  return (
    props.nameValue ||
    itemLabel ||
    (hasSelectedId.value ? String(props.modelValue) : "")
  );
});

const flattenMessages = (value) => {
  if (typeof value === "string") return [value];
  if (Array.isArray(value)) return value.flatMap(flattenMessages);
  if (value && typeof value === "object") {
    return Object.values(value).flatMap(flattenMessages);
  }
  return [];
};

const errorText = computed(() => flattenMessages(props.error).join("، "));
const hasInitialSuggestions = computed(
  () => props.initialSuggestions && results.value.length > 0
);
const canToggleResults = computed(
  () =>
    hasExplicitSearch.value &&
    results.value.length > 0 &&
    !props.loading &&
    !errorText.value
);
const showEmptyState = computed(
  () =>
    hasExplicitSearch.value &&
    hasSearched.value &&
    !props.loading &&
    !errorText.value &&
    results.value.length === 0
);
const activeOptionId = computed(() =>
  showPopup.value && activeIndex.value >= 0
    ? `${listboxId}-option-${activeIndex.value}`
    : ""
);

function clearSelectedValues() {
  selectedItem.value = null;
  emit("update:modelValue", "");
  emit("update:nameValue", "");
  emit("clear");
}

function clearSelection() {
  suppressQueryWatch.value = true;
  query.value = "";
  hasSearched.value = false;
  hasExplicitSearch.value = false;
  responseHandled.value = false;
  currentSearchIsInitial.value = false;
  showPopup.value = false;
  activeIndex.value = -1;
  clearSelectedValues();
  emit("query-change", "");
  nextTick(() => {
    suppressQueryWatch.value = false;
  });
}

function requestSearch(value, isInitial = false) {
  if (!value && !isInitial) {
    if (hasInitialSuggestions.value) {
      hasSearched.value = true;
      hasExplicitSearch.value = true;
      currentSearchIsInitial.value = false;
      responseHandled.value = true;
      showPopup.value = true;
      activeIndex.value = -1;
      return;
    }
    clearSelection();
    return;
  }

  currentSearchIsInitial.value = isInitial;
  hasExplicitSearch.value = !isInitial;
  hasSearched.value = true;
  responseHandled.value = false;
  showPopup.value = false;
  activeIndex.value = -1;
  emit("search", {
    filterType: filterType.value,
    type: filterType.value,
    value,
    initial: isInitial,
  });
}

function onSearch() {
  requestSearch(query.value.trim());
}

function toggleResults() {
  if (!canToggleResults.value) return;
  showPopup.value = !showPopup.value;
  activeIndex.value = -1;
}

function selectItem(item) {
  const id = item?.[props.idField];
  if (id === null || id === undefined || id === "") return;

  suppressQueryWatch.value = true;
  selectedItem.value = item;
  emit("update:modelValue", id);
  emit("update:nameValue", item[props.nameField] ?? "");
  emit("select", item);
  query.value = "";
  hasSearched.value = false;
  hasExplicitSearch.value = false;
  responseHandled.value = false;
  currentSearchIsInitial.value = false;
  showPopup.value = false;
  activeIndex.value = -1;
  nextTick(() => {
    suppressQueryWatch.value = false;
  });
}

function onInputKeydown(event) {
  if (
    event.key === "ArrowDown" &&
    hasExplicitSearch.value &&
    results.value.length > 0
  ) {
    event.preventDefault();
    showPopup.value = true;
    activeIndex.value = Math.min(
      activeIndex.value + 1,
      results.value.length - 1
    );
  } else if (
    event.key === "ArrowUp" &&
    hasExplicitSearch.value &&
    results.value.length > 0
  ) {
    event.preventDefault();
    showPopup.value = true;
    activeIndex.value = activeIndex.value <= 0 ? 0 : activeIndex.value - 1;
  } else if (event.key === "Enter") {
    if (showPopup.value && results.value.length > 0) {
      event.preventDefault();
      selectItem(results.value[Math.max(activeIndex.value, 0)]);
    } else {
      event.preventDefault();
      onSearch();
    }
  } else if (event.key === "Escape" && showPopup.value) {
    event.preventDefault();
    showPopup.value = false;
    activeIndex.value = -1;
  }
}

function updateResultsState() {
  if (!hasSearched.value || props.loading || responseHandled.value) return;

  showPopup.value =
    !currentSearchIsInitial.value &&
    !errorText.value &&
    results.value.length > 0;
  activeIndex.value = -1;

  if (errorText.value || results.value.length === 0) {
    responseHandled.value = true;
    if (!currentSearchIsInitial.value) clearSelectedValues();
    if (!errorText.value) emit("no-results");
  }
}

watch(query, (value, previousValue) => {
  if (suppressQueryWatch.value || value === previousValue) return;

  hasSearched.value = false;
  hasExplicitSearch.value = false;
  responseHandled.value = false;
  currentSearchIsInitial.value = false;
  showPopup.value = false;
  activeIndex.value = -1;
  clearSelectedValues();
  emit("query-change", value);
});

watch(filterType, () => clearSelection());

watch(
  () => props.suggestionKey,
  (key, previousKey) => {
    if (
      !props.initialSuggestions ||
      !key ||
      String(key) === String(previousKey)
    ) {
      return;
    }

    hasSearched.value = false;
    hasExplicitSearch.value = false;
    responseHandled.value = false;
    currentSearchIsInitial.value = false;
    showPopup.value = false;
    activeIndex.value = -1;
    requestSearch("", true);
  },
  { immediate: true }
);

watch(
  () => [props.modelValue, props.nameValue],
  ([modelValue, nameValue]) => {
    if (
      (modelValue === "" || modelValue === null || modelValue === undefined) &&
      !nameValue
    ) {
      selectedItem.value = null;
      return;
    }

    if (
      selectedItem.value &&
      String(selectedItem.value[props.idField]) !== String(modelValue)
    ) {
      selectedItem.value = null;
    }
  }
);

watch(
  () => [props.results, props.loading, props.error, hasSearched.value],
  updateResultsState,
  { deep: true }
);
</script>
