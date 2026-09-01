import { createApp } from "vue";
import { createPinia } from "pinia"; // 1. استيراد createPinia
import App from "./App.vue";
import router from "./router";
import "./css/Main.css";

const app = createApp(App);

app.use(createPinia()); // 2. إنشاء وتفعيل Pinia مباشرة
app.use(router);

app.mount("#app");
