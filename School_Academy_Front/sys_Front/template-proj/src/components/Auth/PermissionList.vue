<template>
  <div>
    <Table>
      <thead>
        <tr>
          <th>رقم</th>
          <th>الصلاحية</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="permission in this.permissions" :key="permission.Id">
          <td>{{ permission.id }}</td>
          <td>{{ permission.name }}</td>
          <td>
            <input
              v-if="CanSelect"
              type="checkbox"
              v-model="permission.isChecked"
              @change="this.selectPermission(permission)"
            />
          </td>
        </tr>
      </tbody>
      <tfoot></tfoot>
    </Table>
    <button @click="getSelectedPermissions">ارسال</button>
  </div>
</template>

<script>
export default {
  name: "PermissionList",
  data() {
    return {
      selectedPermission: [],
    };
  },
  methods: {
    IsSelected(permission) {
      return this.selectedPermission.some((p) => p.id === permission.id);
    },
    selectPermission(permission) {
      if (this.IsSelected(permission)) {
        this.selectedPermission = this.selectedPermission.filter(
          (p) => p.id !== permission.id
        );
      } else {
        const per = structuredClone(permission);
        per.isChecked = !permission.isChecked;
        this.selectedPermission.push(per);
      }
    },
    getSelectedPermissions() {
      this.$emit("getEmit", this.selectedPermission);
    },
    fillSelected(permsList = this.permissions) {
      if (Array.isArray(permsList)) {
        this.selectedPermission = permsList.filter((p) => p.isChecked);
      }
    },
  },

  props: ["permissions", "CanSelect"],
  emits: ["getEmit"],
};
// Selected لا تعبئ
// عند تغيير اي شيء من ال lIST
// اذا كانت True و اصبحت False
// يتم وضعها للحذف و يتم التحقق اذا ارجعها True تزال من الحذف
// اما اذا كانت False واصبحت True  توضع للاضافة
// واذا مزع عنها ال true تنزع من القائمة
</script>
