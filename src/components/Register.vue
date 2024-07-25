<template>
    <div class="register">
      <h1>Register</h1>
      <form @submit.prevent="register">
        <div>
          <label for="username">Username:</label>
          <input type="text" v-model="username" required />
        </div>
        <div>
          <label for="email">Email:</label>
          <input type="email" v-model="email" required />
        </div>
        <div>
          <label for="password">Password:</label>
          <input type="password" v-model="password" required />
        </div>
        <button type="submit">Register</button>
      </form>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: 'Register',
    data() {
      return {
        username: '',
        email: '',
        password: '',
      };
    },
    methods: {
      register() {
        axios.post('http://your-api-endpoint/register', {
          username: this.username,
          email: this.email,
          password: this.password,
        })
        .then(response => {
          console.log('Registration successful:', response.data);
          // 重定向到登录页面或其他逻辑
          this.$router.push({ name: 'Login' });
        })
        .catch(error => {
          console.error('Error registering:', error);
        });
      },
    },
  };
  </script>
  
  <style scoped>
  .register {
    padding: 20px;
  }
  </style>
  