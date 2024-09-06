<template>
  <div class="admin-login">
    <!-- 使用 el-card 组件作为登录卡片的容器 -->
    <el-card class="login-card">
      <h1>Admin Login</h1>
      <!-- el-form 表单组件，用于收集用户输入 -->
      <!-- @submit.prevent="login" 阻止默认的提交事件并调用 login 方法 -->
      <!-- :model="form" 绑定表单数据到 form 对象 -->
      <!-- ref="form" 用于表单的验证 -->
      <!-- label-width="100px" 设置表单标签的宽度 -->
      <el-form @submit.prevent="login" :model="form" ref="form" label-width="100px">
        <!-- 表单项：邮箱 -->
        <!-- :rules 属性定义了表单验证规则 -->
        <el-form-item label="Email" :rules="[{ required: true, message: 'Please input your email', trigger: 'blur' }, { type: 'email', message: 'Please input a valid email', trigger: ['blur', 'change'] }]">
          <!-- el-input 用于输入邮箱，v-model 绑定到 form.email -->
          <el-input type="email" v-model="form.email"></el-input>
        </el-form-item>
        <!-- 表单项：密码 -->
        <el-form-item label="Password" :rules="[{ required: true, message: 'Please input your password', trigger: 'blur' }]">
          <!-- el-input 用于输入密码，v-model 绑定到 form.password -->
          <el-input type="password" v-model="form.password"></el-input>
        </el-form-item>
        <!-- 表单项：登录按钮 -->
        <el-form-item>
          <!-- el-button 登录按钮，点击时调用 submitForm 方法 -->
          <el-button type="primary" @click="submitForm">Login</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script>
import axios from 'axios'; // 引入 axios 用于发送 HTTP 请求

export default {
  name: 'AdminLogin',
  data() {
    return {
      form: {
        email: '',
        password: '',
      },
      isLoggingIn: false,
    };
  },
  methods: {
    resetForm() {
      this.$refs.form.resetFields();
    },
    // 表单提交方法，首先进行表单验证
    submitForm() {
      this.$refs.form.validate((valid) => { // 使用 ref 进行表单验证
        if (valid) {
          this.login(); // 如果验证通过，调用 login 方法
        } else {
          console.log('error submit!!'); // 如果验证失败，输出错误信息
          this.resetForm();
          return false;// 阻止提交
        }
      });
    },
    // 登录方法，向服务器发送登录请求
    login() {
      this.isLoggingIn = true;
      axios.post('http://localhost:5033/admin/login', { // 使用 axios 发送 POST 请求
        email: this.form.email, // 提交邮箱
        password: this.form.password, // 提交密码
      })
      .then(response => {
        localStorage.setItem('adminToken', response.data.token);
        // 登录成功后跳转到管理员页面
        this.$router.push({ name: 'Admin' });
      })
      .catch(error => {
        console.error('Error logging in:', error); // 如果登录失败，打印错误信息
      })
      .finally(() => {
        this.isLoggingIn = false;
      });
    },
  },
};
</script>

<style scoped>
.admin-login {
  padding: 20px; /* 设置内边距 */
  display: flex; /* 使用 flex 布局 */
  justify-content: center; /* 水平居中 */
  align-items: center; /* 垂直居中 */
  height: 100vh; /* 高度设为视口高度 */
}

.login-card {
  width: 400px; /* 登录卡片的宽度 */
}

.login-card h1 {
  text-align: center; /* 标题居中 */
  margin-bottom: 20px; /* 标题下方的间距 */
}
</style>
