import TResult from "../Generic/TResult.js";
export function validate(schema, model) {
  const validations = schema.safeParse(model);
  const result = new TResult();
  if (validations.success) {
    result.data = validations.data;
    result.success = true;
    return result;
  }

  result.success = false;
  result.errors = validations.error.flatten().fieldErrors;
  return result;
}
export function validate1(data, schema) {
  const result = schema.safeParse(data);
  if (!result.success) {
    const formattedErrors = {};

    result.error.issues.forEach((issue) => {
      const path = issue.path.length > 0 ? issue.path : ["_form"];
      let current = formattedErrors;

      path.forEach((segment, index) => {
        const key = String(segment);
        if (index === path.length - 1) {
          const existing = current[key];
          current[key] = existing
            ? Array.isArray(existing)
              ? [...existing, issue.message]
              : [existing, issue.message]
            : issue.message;
          return;
        }

        if (!current[key] || typeof current[key] !== "object") {
          current[key] = {};
        }
        current = current[key];
      });
    });
    return { isValid: false, errors: formattedErrors };
  }
  return { isValid: true, errors: {} };
}
