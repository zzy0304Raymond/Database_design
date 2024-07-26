<template>
  <div class="login">
    <h1>Login</h1>
    <el-form @submit.prevent="login" :model="form" label-position="top" class="login-form">
      <el-form-item label="Email">
        <el-input type="email" v-model="form.email" required></el-input>
      </el-form-item>
      <el-form-item label="Password">
        <el-input type="password" v-model="form.password" required></el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="login">Login</el-button>
      </el-form-item>
    </el-form>
    <p>Don't have an account? <router-link to="/register">Register here</router-link>.</p>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Login',
  data() {
    return {
      form: {
        email: '',
        password: '',
      },
    };
  },
  methods: {
    login() {
      axios.post('http://your-api-endpoint/login', {
        email: this.form.email,
        password: this.form.password,
      })
      .then(response => {
        console.log('Login successful:', response.data);
        // 处理登录成功的逻辑，比如保存用户信息
        this.$router.push({ name: 'Home' });
      })
      .catch(error => {
        console.error('Error logging in:', error);
      });
    },
  },
};
</script>

<style scoped>
.login {
  padding: 20px;
}
.login-form {
  max-width: 400px;
  margin: 0 auto;
}
</style>
