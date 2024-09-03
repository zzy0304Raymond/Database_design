<template>
  <div class="register">
    <!-- 使用 Element Plus 的卡片组件来包裹注册表单 -->
    <el-card class="register-card">
      <h1>Register</h1>
      <!-- 表单组件，绑定到 form 数据对象，设置表单验证规则 -->
      <el-form @submit.prevent="register" :model="form" ref="form" label-width="100px">
        <!-- 用户名输入框，设置必填验证 -->
        <el-form-item label="Username"
          :rules="[{ required: true, message: 'Please input your username', trigger: 'blur' }]">
          <el-input v-model="form.username"></el-input>
        </el-form-item>
        <!-- 邮箱输入框，设置必填和格式验证 -->
        <el-form-item label="Email"
          :rules="[{ required: true, message: 'Please input your email', trigger: 'blur' }, { type: 'email', message: 'Please input a valid email', trigger: ['blur', 'change'] }]">
          <el-input v-model="form.email"></el-input>
        </el-form-item>
        <!-- 密码输入框，设置必填验证 -->
        <el-form-item label="Password"
          :rules="[{ required: true, message: 'Please input your password', trigger: 'blur' }]">
          <el-input type="password" v-model="form.password"></el-input>
        </el-form-item>
        <!-- 提交按钮，调用 submitForm 方法 -->
        <el-form-item>
          <el-button type="primary" @click="submitForm">Register</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script>
import axios from 'axios';

const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL;

export default {
  name: 'Register',
  data() {
    return {
      form: {
        username: '',
        email: '',
        password: '',
      },
      isSubmitting: false, // 增加加载状态标志
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
    async register() {
      if (this.isSubmitting) return; // 防止重复提交
      this.isSubmitting = true;

      try {
        const response = await axios.post(`${BACKEND_BASE_URL}/register`, this.form);

        console.log('Registration successful:', response.data);
        this.$message.success('Registration successful! Please log in.');
        this.$router.push({ name: 'Login' });

      } catch (error) {
        console.error('Error registering:', error);
        if (error.response && error.response.status === 400) {
          this.$message.error('Registration failed. Please check your input.');
        } else {
          this.$message.error('An error occurred. Please try again later.');
        }

      } finally {
        this.isSubmitting = false;
      }
    },
  },
};
</script>

<style scoped>
/* 设置注册页面的样式，使其居中显示 */
.register {
  padding: 20px;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
}

/* 设置注册卡片的宽度 */
.register-card {
  width: 400px;
}

/* 设置标题样式 */
.register-card h1 {
  text-align: center;
  margin-bottom: 20px;
}
</style>
