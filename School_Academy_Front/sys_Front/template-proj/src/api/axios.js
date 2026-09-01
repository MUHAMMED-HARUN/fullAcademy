import axios from "axios";

// 1. تحديد العنوان الرئيسي للـ API
const apiClient = axios.create({
  baseURL: "https://localhost:7017/api", // رابط الـ Backend الخاص بك
  headers: {
    "Content-Type": "application/json",
  },
});

// 2. إرفاق الـ JWT Token تلقائياً مع كل Request
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export default apiClient;
