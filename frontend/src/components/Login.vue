<template>
  <div class="login-page">
    <!-- 左侧文字介绍和背景 -->
    <div class="left-section">
      <h1>AuctionHub.Welcome.</h1>
      <p>Secure Your Dream Deals with Every Smart Bid</p>
    </div>

    <div class="right-section">
      <h1>Login</h1> <!-- 页面标题 -->
      <!-- 登录表单，使用 el-form 组件 -->
      <!-- @submit.prevent="login" 阻止默认提交行为并调用 login 方法 -->
      <!-- :model="form" 绑定表单数据到 form 对象 -->
      <!-- label-position="top" 标签位置在输入框上方 -->
      <el-form @submit.prevent="login" :model="form" label-position="top" class="login-form">
        <!-- 表单项：邮箱输入框 -->
        <el-form-item label="Email">
          <!-- v-model 双向绑定输入框的值到 form.email -->
          <el-input type="email" v-model="form.email" required></el-input>
        </el-form-item>
        <!-- 表单项：密码输入框 -->
        <el-form-item label="Password">
          <!-- v-model 双向绑定输入框的值到 form.password -->
          <el-input type="password" v-model="form.password" required></el-input>
        </el-form-item>
        <!-- 表单项：登录按钮 -->
        <el-form-item>
          <!-- 点击登录按钮时，调用 login 方法 -->
          <el-button type="primary" @click="login">Login</el-button>
        </el-form-item>
      </el-form>
      <!-- 注册链接，引导用户去注册页面 -->
      <p>Don't have an account? <router-link to="/register">Register here</router-link>.</p>
    </div>
  </div>
</template>

<script>
import axios from 'axios'; // 引入 axios 用于发送 HTTP 请求

const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL;

export default {
  name: 'Login', // 组件名称
  data() {
    return {
      form: { // 登录表单数据对象
        email: '', // 用户输入的邮箱
        password: '', // 用户输入的密码
      },
      isLoggingIn: false, // 增加登录状态
    };
  },
  methods: {
    async login() {
      // 防止重复提交
      if (this.isLoggingIn) return;

      this.isLoggingIn = true;

      try {
        // 发送 POST 请求到登录 API，提交用户的邮箱和密码
        const response = await axios.post(`${BACKEND_BASE_URL}/user/login`, {
          email: this.form.email,
          password: this.form.password,
        });

        // 假设后端返回的响应中包含以下数据
        const { token, user } = response.data;

        // 将 token 和用户 ID 存储到本地存储中
        localStorage.setItem('authToken', token);
        localStorage.setItem('userId', user.id);
        localStorage.setItem('username', user.username); // 存储用户名
        localStorage.setItem('userEmail', user.email); // 存储用户邮箱


        // 可选：将用户信息存储到 Vuex 以便全局访问（假设你在使用 Vuex）
        // this.$store.commit('setUser', user);

        // 登录成功后跳转到首页
        this.$router.push({ name: 'Home' });

        this.$message.success('Login successful!');

      } catch (error) {
        // 检查错误响应代码，显示相应的错误信息
        if (error.response && error.response.status === 401) {
          this.$message.error('Invalid credentials. Please try again.');
        } else {
          this.$message.error('An error occurred. Please try again later.');
        }

        console.error('Error logging in:', error);

      } finally {
        this.isLoggingIn = false; // 恢复按钮状态
      }
    },
  },
};
</script>

<style scoped>
.login-page {
  display: flex;
  height: 100vh; /* 让页面高度全屏 */

  background-image: url('@/assets/images/background.jpg'); /* 使用上传的图片作为背景 */
  background-size: cover; /* 使背景图片覆盖整个区域 */
  background-position: center; /* 图片居中显示 */
  background-repeat: no-repeat; /* 防止图片重复 */

}

.left-section {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: flex-start; /* 使文字上移 */
  align-items: flex-end;
  color: rgb(29, 88, 251);
  padding: 100px;
  margin-top: 80px; /* 根据需要调整上移的距离 */
}

.left-section h1 {
  font-size: 2.5rem;
  margin-bottom: 20px;
}

.left-section p {
  font-size: 1.2rem;
  margin-top: 0;
}

.right-section {
  flex: 1; /* 右侧占据一半页面 */
  display: flex;
  flex-direction: column;
  justify-content: flex-start; /* 确保登录框从顶部开始 */
  align-items: center; /* 水平居中 */
  padding: 40px;
  background-color: rgb(243, 243, 243); /* 设置右侧背景色 */
}

h1 {
  text-align: center;
  margin-bottom: 30px;
  margin-top: 100px; /* 可以通过增大或缩小此值调整标题位置 */
}

.login-form {
  max-width: 400px;
  width: 100%;
  padding: 40px;
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
}

.el-form-item {
  margin-bottom: 20px;
}

.el-button {
  width: 100%;
  height: 40px;
}

p {
  text-align: center;
  margin-top: 20px;
  margin-bottom: 0; /* 确保与表单下方没有额外间距 */
}
</style>
