<template>
  <div class="register">
    <!-- 使用 Element Plus 的卡片组件来包裹注册表单 -->
    <el-card class="register-card">
      <h1>Register</h1>
      <!-- 表单组件，绑定到 form 数据对象，设置表单验证规则 -->
      <el-form @submit.prevent="register" :model="form" ref="form" label-width="100px">
        <!-- 用户名输入框，设置必填验证 -->
        <el-form-item label="Username" :rules="[{ required: true, message: 'Please input your username', trigger: 'blur' }]">
          <el-input v-model="form.username"></el-input>
        </el-form-item>
        <!-- 邮箱输入框，设置必填和格式验证 -->
        <el-form-item label="Email" :rules="[{ required: true, message: 'Please input your email', trigger: 'blur' }, { type: 'email', message: 'Please input a valid email', trigger: ['blur', 'change'] }]">
          <el-input v-model="form.email"></el-input>
        </el-form-item>
        <!-- 密码输入框，设置必填验证 -->
        <el-form-item label="Password" :rules="[{ required: true, message: 'Please input your password', trigger: 'blur' }]">
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

export default {
  name: 'Register',
  data() {
    return {
      // 定义表单数据对象，包含用户名、邮箱和密码
      form: {
        username: '',
        email: '',
        password: '',
      },
    };
  },
  methods: {
    // 表单提交方法，首先验证表单的完整性
    submitForm() {
      this.$refs.form.validate((valid) => {
        if (valid) {
          // 验证通过后调用 register 方法进行注册请求
          this.register();
        } else {
          // 验证不通过时打印错误信息
          console.log('error submit!!');
          return false;
        }
      });
    },
    // 注册方法，通过 axios 发送 POST 请求
    register() {
      axios.post('http://your-api-endpoint/register', this.form)
        .then(response => {
          // 注册成功后打印返回信息，并跳转到登录页面
          console.log('Registration successful:', response.data);
          this.$router.push({ name: 'Login' });
        })
        .catch(error => {
          // 注册失败时打印错误信息
          console.error('Error registering:', error);
        });
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
