<template>
  <div class="admin-login">
    <div class="left-section">
      <h1>AuctionHub.Welcome.</h1>
      <p>Secure Your Dream Deals with Every Smart Bid</p>
    </div>

    <div class="right-section">
      <h1>Admin Login</h1> <!-- 页面标题 -->
      <!-- 登录表单，使用 el-form 组件 -->
      <el-form @submit.prevent="login" :model="form" label-position="top" class="login-form">
        <!-- 表单项：邮箱输入框 -->
        <el-form-item label="Email"
          :rules="[{ required: true, message: 'Please input your email', trigger: 'blur' }, { type: 'email', message: 'Please input a valid email', trigger: ['blur', 'change'] }]">
          <el-input type="email" v-model="form.email" required style="width: 100%;"></el-input>
        </el-form-item>
        <!-- 表单项：密码输入框 -->
        <el-form-item label="Password"
          :rules="[{ required: true, message: 'Please input your password', trigger: 'blur' }]">
          <el-input type="password" v-model="form.password" required style="width: 100%;"></el-input>
        </el-form-item>
        <!-- 表单项：登录按钮 -->
        <el-form-item>
          <el-button type="primary" @click="submitForm" style="width: 100%;">Login</el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<!-- <script>
import axios from 'axios'; // 引入 axios 用于发送 HTTP 请求

const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL; -->

// export default {
//   name: 'AdminLogin',
//   data() {
//     return {
//       form: {
//         email: '',
//         password: '',
//       },
//       isLoggingIn: false,
//     };
//   },
//   methods: {
//     resetForm() {
//       this.$refs.form.resetFields(); // 仍然保留表单重置功能
//     },
//     // 表单提交方法，不再进行验证
//     submitForm() {
//       this.login(); // 直接调用 login 方法，不进行表单验证
//     },
//     // 登录方法，向服务器发送登录请求
//     login() {
//       this.isLoggingIn = true;
//       axios.post(`${BACKEND_BASE_URL}/admin/login`, {
//         email: this.form.email, // 提交邮箱
//         password: this.form.password, // 提交密码
//       })
//         .then(response => {
//           localStorage.setItem('adminToken', response.data.token);
//           // 登录成功后跳转到管理员页面
//           this.$router.push({ name: 'Admin' });
//         })
//         .catch(error => {
//           console.error('Error logging in:', error); // 如果登录失败，打印错误信息
//         })
//         .finally(() => {
//           this.isLoggingIn = false;
//         });
//     },
//   },
// };
<!-- // </script> -->

<script>
import axios from 'axios'; // 引入 axios 用于发送 HTTP 请求

const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL;

export default {
  name: 'AdminLogin',
  data() {
    return {
      form: {
        email: '',
        password: '',
      },
      isLoggingIn: false, // 控制登录状态
    };
  },
  methods: {
    resetForm() {
      this.$refs.form.resetFields(); // 保留表单重置功能
    },

    // 提交表单方法，调用 login 方法进行登录
    submitForm() {
      this.login(); 
    },

    // 登录方法，向服务器发送登录请求
    async login() {
      this.isLoggingIn = true;
      try {
        const response = await axios.post(`${BACKEND_BASE_URL}/admin/login`, {
          email: this.form.email, // 提交邮箱
          password: this.form.password, // 提交密码
        });

        // 如果登录成功，后端会返回成功的 message
        if (response.status === 200) {
          // 设置管理员登录状态
          sessionStorage.setItem('isAdminLoggedIn', 'true');
          
          this.$message.success('Login successfully'); // 显示登录成功的提示
          this.$router.push({ name: 'Admin' }); // 跳转到管理员页面
        } else {
          // 如果后端返回其他信息，处理错误提示
          this.$message.error(response.data.message || 'Login failed');
        }
      } catch (error) {
        // 处理请求过程中可能发生的错误
        console.error('Error logging in:', error);
        this.$message.error('An error occurred during login. Please try again.');
      } finally {
        this.isLoggingIn = false; // 登录结束，解除加载状态
      }
    },
  },
};
</script>



<style scoped>
.admin-login {
  display: flex;
  height: 100vh; /* 让页面高度全屏 */

  background-image: url('@/assets/images/background.jpg'); /* 使用上传的图片作为背景 */
  background-size: cover; /* 使背景图片覆盖整个区域 */
  background-position: center; /* 图片居中显示 */
  background-repeat: no-repeat; /* 防止图片重复 */

}

.left-section {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: flex-start; /* 使文字上移 */
  align-items: flex-end;
  color: rgb(29, 88, 251);
  padding: 100px;
  margin-top: 80px; /* 根据需要调整上移的距离 */
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
  flex: 1; /* 右侧占据一半页面 */
  display: flex;
  flex-direction: column;
  justify-content: flex-start; /* 确保登录框从顶部开始 */
  align-items: center; /* 水平居中 */
  padding: 40px;
  background-color: rgb(243, 243, 243); /* 设置右侧背景色 */
}

h1 {
  text-align: center;
  margin-bottom: 30px;
  margin-top: 100px; /* 可以通过增大或缩小此值调整标题位置 */
}

.login-form {
  max-width: 400px;
  width: 100%;
  padding: 40px;
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
}

.el-form-item {
  margin-bottom: 20px;
}

.el-button {
  width: 100%;
  height: 40px;
}

p {
  text-align: center;
  margin-top: 20px;
  margin-bottom: 0; /* 确保与表单下方没有额外间距 */
}
</style>
