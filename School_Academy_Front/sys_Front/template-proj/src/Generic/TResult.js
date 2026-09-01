export default class TResult {
  constructor(data = null, success = false, errors = {}) {
    this.data = data;
    this.success = success;
    this.errors = errors;
  }
}
