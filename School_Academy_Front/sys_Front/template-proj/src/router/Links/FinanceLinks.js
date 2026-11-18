import FeeStructureView from "@/views/Finance/FeeStructureView.vue";
import PaymentView from "@/views/Finance/PaymentView.vue";
import StudentInvoiceView from "@/views/Finance/StudentInvoiceView.vue";

export const FinanceLinks = [
  {
    path: "/feeStructure",
    name: "FeeStructure",
    component: FeeStructureView,
    meta: {
      requiresAuth: true,
      requiredPermission: "Can_Read_FeeStructure",
    },
  },
  {
    path: "/studentInvoice",
    name: "StudentInvoice",
    component: StudentInvoiceView,
    meta: {
      requiresAuth: true,
      requiredPermission: "Can_Read_StudentInvoice",
    },
  },
  {
    path: "/payment",
    name: "Payment",
    component: PaymentView,
    meta: {
      requiresAuth: true,
      requiredPermission: "Can_Read_Payment",
    },
  },
];
