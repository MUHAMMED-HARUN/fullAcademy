const { defineConfig } = require("@vue/cli-service");
module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    // حل مشكلة WebSocket في Vue CLI
    client: {
      webSocketURL: "ws://localhost:8080/ws",
    },
  },
});
