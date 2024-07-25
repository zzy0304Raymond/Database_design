<template>
    <div class="admin-login">
      <h1>Admin Login</h1>
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
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: 'AdminLogin',
    data() {
      return {
        email: '',
        password: '',
      };
    },
    methods: {
      login() {
        axios.post('http://your-api-endpoint/admin/login', {
          email: this.email,
          password: this.password,
        })
        .then(response => {
          console.log('Admin login successful:', response.data);
          // 假设成功后返回管理员令牌
          localStorage.setItem('adminToken', response.data.token);
          this.$router.push({ name: 'Admin' });
        })
        .catch(error => {
          console.error('Error logging in:', error);
        });
      },
    },
  };
  </script>
  
  <style scoped>
  .admin-login {
    padding: 20px;
  }
  </style>
  