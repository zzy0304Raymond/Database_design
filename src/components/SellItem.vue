<template>
    <div class="sell-item-container">
      <h1>卖出商品</h1>
      <el-form :model="item" ref="itemForm" label-width="120px">
        <el-form-item label="商品名称" :rules="[{ required: true, message: '请输入商品名称', trigger: 'blur' }]">
          <el-input v-model="item.name"></el-input>
        </el-form-item>
        <el-form-item label="起拍价" :rules="[{ required: true, message: '请输入起拍价', trigger: 'blur' }]">
          <el-input-number v-model="item.startingBid" :min="0"></el-input-number>
        </el-form-item>
        <el-form-item label="商品分类" :rules="[{ required: true, message: '请选择商品分类', trigger: 'change' }]">
          <el-select v-model="item.category" placeholder="请选择分类">
            <el-option label="Antiques" value="Antiques"></el-option>
            <el-option label="Art" value="Art"></el-option>
            <el-option label="Electronics" value="Electronics"></el-option>
            <el-option label="Jewelry" value="Jewelry"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="商品描述">
          <el-input type="textarea" v-model="item.description"></el-input>
        </el-form-item>
        <el-form-item label="上传图片" :rules="[{ required: true, message: '请上传商品图片', trigger: 'change' }]">
          
                      <!-- action="http://your-api-endpoint/upload" -->
          <el-upload

            list-type="picture-card"
            :on-preview="handlePreview"
            :on-remove="handleRemove"
            :on-success="handleSuccess"
            :file-list="item.images">
            <i class="el-icon-plus"></i>
          </el-upload>
        </el-form-item>
        <el-button type="primary" @click="submitForm">提交</el-button>
      </el-form>
  
      <el-dialog :visible.sync="dialogVisible">
        <img :src="dialogImageUrl" alt="" style="width: 100%;" />
      </el-dialog>
    </div>
  </template>
  


  <script>
  import axios from 'axios';
  export default {
    data() {
      return {
        item: {
          name: '',
          startingBid: null,
          category: '',
          description: '',
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
            // 提交表单数据到服务器
            // axios.post('http://your-api-endpoint/auction-items', this.item)
            //   .then(() => {
            //     this.$message.success('商品发布成功');
            //     this.$router.push({ name: 'Home' });
            //   })
            //   .catch((error) => {
            //     this.$message.error('发布失败，请重试');
            //     console.error(error);
            //   });
          } else {
            this.$message.error('请填写完整的商品信息');
          }
        });
      },
    },
  };
  </script>
  
  <style scoped>
  .sell-item-container {
    max-width: 600px;
    margin: 0 auto;
    padding: 20px;
    background-color: #fff;
    border-radius: 10px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  }
  </style>
  