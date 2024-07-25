<template>
    <div class="login">
      <h1>Login</h1>
      <form @submit.prevent="login">
        <div>
          <label for="email">Email:</label>
          <input type="email" v-model="email" required />
        </div>
        <div>
          <label for="password">Password:</label>
          <input type="password" v-model="password" required />
        </div>
        <button type="submit">Login</button>
      </form>
      <p>Don't have an account? <router-link to="/register">Register here</router-link>.</p>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: 'Login',
    data() {
      return {
        email: '',
        password: '',
      };
    },
    methods: {
      login() {
        axios.post('http://your-api-endpoint/login', {
          email: this.email,
          password: this.password,
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
  </style>
  