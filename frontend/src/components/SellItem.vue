<template>
  <div class="sell-item-container">
    <h1>卖出商品</h1>
    <el-form :model="item" ref="itemForm" label-width="120px">
      <!-- 商品名称 -->
      <el-form-item label="商品名称" :rules="[{ required: true, message: '请输入商品名称', trigger: 'blur' }]">
        <el-input v-model="item.name"></el-input>
      </el-form-item>

      <!-- 起拍价 -->
      <el-form-item label="起拍价" :rules="[{ required: true, message: '请输入起拍价', trigger: 'blur' }]">
        <el-input-number v-model="item.startingBid" :min="0"></el-input-number>
      </el-form-item>

      <!-- 商品分类 -->
      <el-form-item label="商品分类" :rules="[{ required: true, message: '请选择商品分类', trigger: 'change' }]">
        <el-select v-model="item.category" placeholder="请选择分类">
          <el-option label="Antiques" value="Antiques"></el-option>
          <el-option label="Art" value="Art"></el-option>
          <el-option label="Electronics" value="Electronics"></el-option>
          <el-option label="Jewelry" value="Jewelry"></el-option>
        </el-select>
      </el-form-item>

      <!-- 商品描述 -->
      <el-form-item label="商品描述">
        <el-input type="textarea" v-model="item.description"></el-input>
      </el-form-item>

      <!-- 上传图片 -->
      <el-form-item label="上传图片" :rules="[{ required: true, message: '请上传商品图片', trigger: 'change' }]">
        <el-upload
          action="http://your-api-endpoint/upload"
          list-type="picture-card"
          :on-preview="handlePreview"
          :on-remove="handleRemove"
          :on-success="handleSuccess"
          :file-list="item.images">
          <i class="el-icon-plus"></i>
        </el-upload>
      </el-form-item>

      <!-- 提交按钮 -->
      <el-button type="primary" @click="submitForm">提交</el-button>
    </el-form>

    <!-- 图片预览对话框 -->
    <el-dialog :visible.sync="dialogVisible">
      <img :src="dialogImageUrl" alt="" style="width: 100%;" />
    </el-dialog>
  </div>
</template>

<script>
import axios from 'axios';

const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL;

export default {
  data() {
    return {
      item: {
        name: '',
        startingBid: null,
        category: '',
        postTime: '',
        images: [],
      },
      dialogImageUrl: '',
      dialogVisible: false,
    };
  },
  methods: {
    handlePreview(file) {
      this.dialogImageUrl = file.url;
      this.dialogVisible = true;
    },
    handleRemove(file, fileList) {
      this.item.images = fileList;
    },
    handleSuccess(response, file) {
      this.item.images.push(response.url);
    },
    submitForm() {
      this.$refs.itemForm.validate((valid) => {
        if (valid) {
          axios.post(`${BACKEND_BASE_URL}/auction-items`, this.item)
            .then(() => {
              this.$message.success('商品发布成功');
              this.$router.push({ name: 'Home' });
            })
            .catch((error) => {
              this.$message.error('发布失败，请重试');
              console.error(error);
            });
        } else {
          this.$message.error('请填写完整的商品信息');
        }
      });
    },
  },
};
</script>

<style scoped>
/* 页面整体布局，包含背景图片 */
.sell-item-container {
  width: 100%;
  height: 100vh; /* 设置整个容器的高度为视口高度 */
  background-image: url('@/assets/images/background.jpg'); /* 背景图片 */
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
}

/* 标题样式 */
h1 {
  text-align: center;
  margin-bottom: 30px;
  font-family: 'Helvetica Neue', Arial, sans-serif;
  color: #2c3e50;
  margin-top: 0; /* 去除顶部空白 */
  padding-top: 100px; /* 视需要可以调整这个值，控制上下边距 */
}

/* 表单样式 */
.el-form {
  max-width: 600px;
  width: 100%;
  margin: 0 auto;
  padding: 20px;
  background-color: rgba(255, 255, 255, 0.8); /* 半透明背景 */
  border-radius: 10px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

/* 表单项样式 */
.el-form-item {
  margin-bottom: 20px;
}

/* 输入框和选择框 */
.el-input,
.el-select,
.el-input-number,
.el-textarea {
  width: 100%;
  max-width: 500px;
}

/* 提交按钮样式 */
.el-button {
  display: block;
  width: 100%;
  max-width: 300px;
  margin: 30px auto 0;
  font-size: 16px;
  background-color: #056bd2;
  color: white;
  border-radius: 25px;
  transition: background-color 0.3s ease;
}

.el-button:hover {
  background-color: #025aa5;
}

/* 上传图片组件样式 */
.el-upload {
  width: 100%;
  max-width: 500px;
  margin: 0 auto;
}

.el-upload .el-icon-plus {
  font-size: 28px;
  color: #999;
}

.el-upload:hover .el-icon-plus {
  color: #056bd2;
}

.el-upload-list {
  width: 100%;
}

.el-upload-list__item {
  max-width: 200px;
}
</style>
