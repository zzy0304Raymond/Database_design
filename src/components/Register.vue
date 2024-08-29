<template>
  <div class="register">
    <el-card class="register-card">
      <h1>Register</h1>
      <el-form @submit.prevent="register" :model="form" ref="form" label-width="100px">
        <el-form-item label="Username" :rules="[{ required: true, message: 'Please input your username', trigger: 'blur' }]">
          <el-input v-model="form.username"></el-input>
        </el-form-item>
        <el-form-item label="Email" :rules="[{ required: true, message: 'Please input your email', trigger: 'blur' }, { type: 'email', message: 'Please input a valid email', trigger: ['blur', 'change'] }]">
          <el-input v-model="form.email"></el-input>
        </el-form-item>
        <el-form-item label="Password" :rules="[{ required: true, message: 'Please input your password', trigger: 'blur' }]">
          <el-input type="password" v-model="form.password"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="submitForm">Register</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Register',
  data() {
    return {
      form: {
        username: '',
        email: '',
        password: '',
      },
    };
  },
  methods: {
    submitForm() {
      this.$refs.form.validate((valid) => {
        if (valid) {
          this.register();
        } else {
          console.log('error submit!!');
          return false;
        }
      });
    },
    register() {
      axios.post('http://your-api-endpoint/register', this.form)
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
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
}

.register-card {
  width: 400px;
}

.register-card h1 {
  text-align: center;
  margin-bottom: 20px;
}
</style>
