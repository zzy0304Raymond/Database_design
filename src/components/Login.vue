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
    };
  },
  methods: {
    // 登录方法，处理用户登录逻辑
    login() {
      // 发送 POST 请求到登录 API，提交用户的邮箱和密码
      axios.post('http://your-api-endpoint/login', {
        email: this.form.email, // 提交的邮箱
        password: this.form.password, // 提交的密码
      })
      .then(response => {
        console.log('Login successful:', response.data); // 打印登录成功的响应数据
        // 处理登录成功后的逻辑，比如保存用户信息到本地存储或状态管理
        this.$router.push({ name: 'Home' }); // 登录成功后跳转到首页
      })
      .catch(error => {
        console.error('Error logging in:', error); // 打印登录失败的错误信息
        // 处理登录失败的逻辑，比如显示错误消息
      });
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
