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
      const field = issue.path[0];
      formattedErrors[field] = issue.message;
    });
    return { isValid: false, errors: formattedErrors };
  }
  return { isValid: true, errors: {} };
}
