export function applyClientQuery(
  items,
  { rawValues = {}, orderBy = null, page = 1, pageSize = 10 } = {},
  options = {}
) {
  const containsFields = options.containsFields || [];
  const equalsFields = options.equalsFields || [];
  const gteFields = options.gteFields || {};
  const lteFields = options.lteFields || {};
  const values = rawValues || {};

  let result = Array.isArray(items) ? [...items] : [];

  containsFields.forEach((field) => {
    const val = values[field];
    if (val) {
      const needle = String(val).toLowerCase();
      result = result.filter((item) =>
        String(item[field] ?? "")
          .toLowerCase()
          .includes(needle)
      );
    }
  });

  equalsFields.forEach((field) => {
    const val = values[field];
    if (val !== null && val !== "" && val !== undefined) {
      result = result.filter((item) => String(item[field]) === String(val));
    }
  });

  Object.entries(gteFields).forEach(([filterKey, itemField]) => {
    const val = values[filterKey];
    if (val) {
      result = result.filter(
        (item) => String(item[itemField] || "") >= String(val)
      );
    }
  });

  Object.entries(lteFields).forEach(([filterKey, itemField]) => {
    const val = values[filterKey];
    if (val) {
      result = result.filter(
        (item) => String(item[itemField] || "") <= String(val)
      );
    }
  });

  if (orderBy) {
    const [field, dir] = String(orderBy).split(" ");
    const desc = (dir || "asc").toLowerCase() === "desc";
    result.sort((a, b) => {
      const av = a[field];
      const bv = b[field];
      if (av == null && bv == null) return 0;
      if (av == null) return 1;
      if (bv == null) return -1;
      if (av < bv) return desc ? 1 : -1;
      if (av > bv) return desc ? -1 : 1;
      return 0;
    });
  }

  const count = result.length;
  const currentPage = page || 1;
  const size = pageSize || 10;
  const start = (currentPage - 1) * size;

  return {
    items: result.slice(start, start + size),
    count,
  };
}
