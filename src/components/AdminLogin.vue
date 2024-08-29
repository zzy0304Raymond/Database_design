<template>
  <div class="admin-login">
    <el-card class="login-card">
      <h1>Admin Login</h1>
      <el-form @submit.prevent="login" :model="form" ref="form" label-width="100px">
        <el-form-item label="Email" :rules="[{ required: true, message: 'Please input your email', trigger: 'blur' }, { type: 'email', message: 'Please input a valid email', trigger: ['blur', 'change'] }]">
          <el-input type="email" v-model="form.email"></el-input>
        </el-form-item>
        <el-form-item label="Password" :rules="[{ required: true, message: 'Please input your password', trigger: 'blur' }]">
          <el-input type="password" v-model="form.password"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="submitForm">Login</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'AdminLogin',
  data() {
    return {
      form: {
        email: '',
        password: '',
      },
    };
  },
  methods: {
    submitForm() {
      this.$refs.form.validate((valid) => {
        if (valid) {
          this.login();
        } else {
          console.log('error submit!!');
          return false;
        }
      });
    },
    login() {
      axios.post('http://your-api-endpoint/admin/login', {
        email: this.form.email,
        password: this.form.password,
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
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
}

.login-card {
  width: 400px;
}

.login-card h1 {
  text-align: center;
  margin-bottom: 20px;
}
</style>
