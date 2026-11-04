const { defineConfig } = require("@vue/cli-service");
module.exports = defineConfig({
  transpileDependencies: true,
  lintOnSave: "warning", // أو false لتعطيل الفحص بالكامل أثناء التطوير
  devServer: {
    // حل مشكلة WebSocket في Vue CLI
    client: {
      webSocketURL: "ws://localhost:8080/ws",
    },
  },
});
