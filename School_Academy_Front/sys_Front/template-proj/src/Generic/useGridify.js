import { ref, reactive, computed } from "vue";

export function useGridify(fetchCallback, initialFilters = {}) {
  const page = ref(1);
  const pageSize = ref(10);
  const orderBy = ref("");
  const filters = reactive({ ...initialFilters });

  // قائمة القواعد والمعاملات حسب نهاية اسم الحقل
  const operatorsMap = [
    { suffix: "GreaterThanOrEqual", op: ">=" },
    { suffix: "LessThanOrEqual", op: "<=" },
    { suffix: "GreaterThan", op: ">" },
    { suffix: "LessThan", op: "<" },
    { suffix: "Gte", op: ">=" }, // اختصار Greater Than or Equal
    { suffix: "Lte", op: "<=" }, // اختصار Less Than or Equal
    { suffix: "Gt", op: ">" }, // اختصار Greater Than
    { suffix: "Lt", op: "<" }, // اختصار Less Than
  ];

  const filterString = computed(() => {
    const rules = [];

    for (const [key, value] of Object.entries(filters)) {
      if (value === null || value === undefined || value === "") continue;

      // 1. فحص ما إذا كان اسم الحقل ينتهي بإحدى كلمات المقارنة
      const matchedOp = operatorsMap.find((item) => key.endsWith(item.suffix));

      if (matchedOp) {
        // استخراج اسم الحقل الأصلي بعد حذف السحقة (مثلاً BirthDate)
        const fieldName = key.slice(0, -matchedOp.suffix.length);
        rules.push(`${fieldName}${matchedOp.op}${value}`);
      }
      // 2. الحالات العادية (أرقام، بولين، نصوص)
      else if (typeof value === "boolean" || typeof value === "number") {
        rules.push(`${key}=${value}`);
      } else if (typeof value === "string") {
        rules.push(`${key}=*${value.trim()}`);
      }
    }

    return rules.join(",");
  });

  const queryParams = computed(() => ({
    page: page.value,
    pageSize: pageSize.value,
    orderBy: orderBy.value || undefined,
    filter: filterString.value || undefined,
  }));

  const loadData = async () => {
    return await fetchCallback(queryParams.value);
  };

  const resetFilters = () => {
    Object.keys(filters).forEach(
      (key) => (filters[key] = initialFilters[key] ?? "")
    );
    page.value = 1;
    loadData();
  };

  return {
    page,
    pageSize,
    orderBy,
    filters,
    queryParams,
    loadData,
    resetFilters,
  };
}
