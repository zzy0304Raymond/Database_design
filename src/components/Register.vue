<template>
  <div class="register-page">
    <!-- 左侧文字介绍和背景 -->
    <div class="left-section">
      <h1>AuctionHub.Welcome.</h1>
      <p>Secure Your Dream Deals with Every Smart Bid</p>
    </div>

    <!-- 右侧注册表单 -->
    <div class="right-section">
      <h1>Register</h1>
      <el-card class="register-card">
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
        const response = await axios.post(`${BACKEND_BASE_URL}/user/register`, this.form);

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
.register-page {
  display: flex;
  height: 100vh; /* 页面高度全屏 */
  background-image: url('@/assets/images/background.jpg'); /* 背景图片 */
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
}

.left-section {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: flex-end;
  color: rgb(29, 88, 251);
  padding: 100px;
  margin-top: 80px;
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
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding: 40px;
  background-color: rgb(243, 243, 243);
  margin-top: -200px; /* 向上移动内容 */
}

.register-card {
  max-width: 400px;
  width: 100%;
  padding: 40px;
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1); /* 卡片阴影效果 */
}

h1 {
  text-align: center;
  margin-bottom: 30px;
  margin-top: 100px;
}

.login-form {
  max-width: 400px;
  width: 100%;
  padding: 40px;
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
}

.login-form .el-form-item {
  margin-bottom: 30px; /* 增加表单项之间的间距 */
}

.el-form-item {
  margin-bottom: 20px;
}

.el-button {
  height: 45px; /* 增加按钮高度 */
  background-color: #409eff; /* 蓝色按钮背景 */
  border-color: #409eff; /* 按钮边框颜色 */
  color: white; /* 按钮文字颜色 */
  font-size: 16px; /* 按钮文字大小 */
}

.el-button:hover {
  background-color: #66b1ff; /* 鼠标悬停时按钮颜色变浅 */
}

.el-input, .el-button {
  width: 100%; /* 使输入框和按钮的宽度一致 */
}

p {
  text-align: center;
  margin-top: 20px;
  margin-bottom: 0;
}
</style>
