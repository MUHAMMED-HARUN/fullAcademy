<template>
  <div>
    <Table>
      <thead>
        <tr>
          <th>رقم</th>
          <th>اسم الدور</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(Role, index) in this.Roles" :key="Role.Id">
          <td>{{ index + 1 }}</td>
          <td>{{ Role.Name }}</td>
          <td>
            <button @click="OnEditPermission(Role)">تعديل صلاحيات</button>
          </td>
        </tr>
      </tbody>
    </Table>
    <PopapComp @close="showPopap = false" v-if="showPopap"
      ><PermissionList
        @get-emit="EmitUpdatedPermissions"
        :-can-select="true"
        :permissions="this.Permissions"
    /></PopapComp>
  </div>
</template>

<script>
import PopapComp from "../general/popapComp.vue";
import PermissionList from "./PermissionList.vue";
import { useAuthStore } from "@/store/Auth/authStore.js";

export default {
  name: "RoleList",
  data() {
    return {
      auth: useAuthStore(),
      Roles: null,
      SelectedRole: null,
      Permissions: null,
      showPopap: false,
      RolePermissions: null,
    };
  },
  methods: {
    async OnEditPermission(e) {
      console.clear();
      this.SelectedRole = e;
      this.showPopap = true;
      // مراعاة حالة الأحرف (e.ID أو e.Id)
      const roleId = e.ID || e.Id;
      // 1. جلب البيانات في نفس الوقت لتسريع الاستجابة (Parallel Fetching)
      const [rolePermsRes, permsRes] = await Promise.all([
        this.auth.getAllRolePermissions(roleId),
        this.auth.getPermissions(),
      ]);
      console.log(rolePermsRes);
      console.log(permsRes);

      // 2. التحقق من نجاح العمليتين
      if (rolePermsRes?.success && permsRes?.success) {
        const rolePermissions = rolePermsRes.data || [];
        const allPermissions = permsRes.data || [];

        // 3. تحويل الصلاحيات إلى الشكل النهائي: { id, name, isChecked }
        this.Permissions = allPermissions.map((p) => ({
          id: p.Id || p.ID,
          name: p.Name,
          isChecked: rolePermissions.some(
            (rp) =>
              rp.PermissionName === p.Name || rp.PermissionName.includes(p.Name)
          ),
        }));
      }
    },
    EmitUpdatedPermissions(e) {
      const changesArr = e.map((p) => ({
        roleId: this.SelectedRole.ID,
        permissionName: p.name,
        isDelete: Boolean(p.isChecked),
      }));
      const payload = { changes: changesArr };
      this.auth.UpdateRolePermissions(payload);
    },
  },
  async mounted() {
    this.Roles = (await this.auth.getRoles()).data;
  },
  components: {
    PopapComp,
    PermissionList,
  },
};
</script>
