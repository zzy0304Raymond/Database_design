<template>
  <div class="login">
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
</template>

<script>
import axios from 'axios'; // 引入 axios 用于发送 HTTP 请求

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
        const response = await axios.post('http://your-api-endpoint/login', {
          email: this.form.email,
          password: this.form.password,
        });

        // 假设后端返回的响应中包含以下数据
        const { token, user } = response.data;

        // 将 token 和用户 ID 存储到本地存储中
        localStorage.setItem('authToken', token); 
        localStorage.setItem('userId', user.id);

        // 可选：将用户信息存储到 Vuex 以便全局访问（假设你在使用 Vuex）
        this.$store.commit('setUser', user);

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
.login {
  padding: 20px; /* 设置页面内边距 */
}
.login-form {
  max-width: 400px; /* 设置表单的最大宽度 */
  margin: 0 auto; /* 水平居中显示表单 */
}
</style>
