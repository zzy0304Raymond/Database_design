<template>
  <div class="admin">
    <h1>Admin Panel</h1>
    <div class="add-item">
      <h2>Add New Item</h2>
      <el-form @submit.prevent="addItem" :model="newItem" ref="newItemForm">
        <el-form-item label="Name"
          :rules="[{ required: true, message: 'Please input the item name', trigger: 'blur' }]">
          <el-input v-model="newItem.name"></el-input>
        </el-form-item>
        <el-form-item label="Starting Bid"
          :rules="[{ required: true, message: 'Please input the starting bid', trigger: 'blur' }]">
          <el-input type="number" v-model="newItem.startingBid"></el-input>
        </el-form-item>
        <el-form-item label="Category"
          :rules="[{ required: true, message: 'Please input the category', trigger: 'blur' }]">
          <el-input v-model="newItem.category"></el-input>
        </el-form-item>
        <el-form-item label="End Time"
          :rules="[{ required: true, message: 'Please select the end time', trigger: 'change' }]">
          <el-date-picker v-model="newItem.endTime" type="datetime" placeholder="Select date and time"></el-date-picker>
        </el-form-item>
        
        <!-- 图片选择器 -->
        <el-form-item label="Image">
          <input type="file" @change="onFileChange" />
        </el-form-item>

        <el-form-item>
          <el-button type="primary" :loading="isAddingItem" @click="submitNewItemForm">Add Item</el-button>
          <el-button @click="resetNewItemForm">Reset</el-button>
        </el-form-item>
      </el-form>
    </div>

    <!-- 编辑表单，直接展示在页面上 -->
    <div class="edit-item" v-if="editingItem">
      <h2>Edit Item</h2>
      <el-form :model="editingItem" ref="editItemForm">
        <el-form-item label="Name">
          <el-input v-model="editingItem.name"></el-input>
        </el-form-item>
        <el-form-item label="Starting Bid">
          <el-input type="number" v-model="editingItem.startingBid"></el-input>
        </el-form-item>
        <el-form-item label="Category">
          <el-input v-model="editingItem.category"></el-input>
        </el-form-item>
        <el-form-item label="End Time">
          <el-date-picker v-model="editingItem.endTime" type="datetime" placeholder="Select date and time"></el-date-picker>
        </el-form-item>

        <!-- 图片选择器 -->
        <el-form-item label="Image">
          <input type="file" @change="onFileChange" />
        </el-form-item>

        <el-form-item>
          <el-button type="primary" :loading="isUpdatingItem" @click="submitEditItemForm">Update Item</el-button>
        </el-form-item>
      </el-form>
    </div>

    <div class="item-list">
      <h2>Item List</h2>
      <el-table :data="auctionItems" style="width: 100%">
        <el-table-column prop="name" label="Item Name"></el-table-column>
        <el-table-column prop="startingBid" label="Starting Bid"></el-table-column>
        <el-table-column label="Actions">
          <template #default="scope">
            <el-button @click="editItem(scope.row.id)">Edit</el-button>
            <el-button type="danger" @click="deleteItem(scope.row.id)">Delete</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL;

export default {
  name: 'Admin',
  data() {
    return {
      newItem: {
        name: '',
        startingBid: 0,
        category: '',
        endTime: '',
        ImageUrl: '',
      },
      auctionItems: [],
      editingItem: {
        name: '',
        startingBid: 0,
        category: '',
        endTime: '',
        ImageUrl: '',
      },
      isAddingItem: false,
      isUpdatingItem: false,
    };
  },
  created() {
    this.fetchAuctionItems();
  },
  methods: {
      // 处理文件选择并转换为Base64
      onFileChange(e) {
        const file = e.target.files[0];
        if (file) {
          const reader = new FileReader();
          reader.onload = (event) => {
            const base64String = event.target.result;
            // 使用split方法去除逗号之前的内容，只保留纯粹的Base64编码部分
            const pureBase64 = base64String.split(',')[1];
            if (this.editingItem) {
              this.editingItem.ImageUrl = pureBase64; // 编辑模式下
            } else {
              this.newItem.ImageUrl = pureBase64; // 添加模式下
            }
          };
          reader.readAsDataURL(file); // 将文件转换为Base64
        }
      },
    fetchAuctionItems() {
      axios.get(`${BACKEND_BASE_URL}/auction-items`)
        .then(response => {
          this.auctionItems = response.data;
        })
        .catch(error => {
          this.$message.error('Error fetching auction items.');
          console.error('Error fetching auction items:', error);
        });
    },
    submitNewItemForm() {
      this.$refs.newItemForm.validate((valid) => {
        if (valid) {
          this.addItem();
        } else {
          console.log('error submit!!');
          return false;
        }
      });
    },
    formatDateTime(date) {
      const d = new Date(date);
      return d.getFullYear() + '-' + 
        String(d.getMonth() + 1).padStart(2, '0') + '-' + 
        String(d.getDate()).padStart(2, '0') + ' ' + 
        String(d.getHours()).padStart(2, '0') + ':' + 
        String(d.getMinutes()).padStart(2, '0') + ':' + 
        String(d.getSeconds()).padStart(2, '0');
    },
    addItem() {
      this.isAddingItem = true;
      this.newItem.endTime = this.formatDateTime(this.newItem.endTime);
      //这里response改成ItemsID
      // 提交包含Base64图片的表单数据
      axios.post(`${BACKEND_BASE_URL}/auction-items`, this.newItem)
        .then(response => {
          const addedItem = response.data;  // 获取返回的完整数据，包括 itemId
          console.log('Item added:', addedItem);  // 调试输出
          this.auctionItems.push(response.data);
          this.resetNewItemForm();
        })
        .catch(error => {
          this.$message.error('Error adding item.');
          console.error('Error adding item:', error);
        })
        .finally(() => {
          this.isAddingItem = false;
        });
    },
    resetNewItemForm() {
      this.newItem = { // 重置 newItem 数据对象
        name: '',
        startingBid: 0,
        category: '',
        endTime: '',
        ImageUrl: '',
      };
      
      // 清除表单的验证状态
      if (this.$refs.newItemForm) {
        this.$refs.newItemForm.clearValidate();
      }
    },
    editItem(id) {
      const item = this.auctionItems.find(item => item.id === id);
      this.editingItem = { ...item };
      this.editItemDialogVisible = true;
    },
    submitEditItemForm() {
      this.$refs.editItemForm.validate((valid) => {
        if (valid) {
          this.updateItem();
        } else {
          console.log('error submit!!');
          return false;
        }
      });
    },
    updateItem() {
      this.isUpdatingItem = true;
      this.editingItem.endTime = this.formatDateTime(this.editingItem.endTime);
      

      axios.put(`${BACKEND_BASE_URL}/auction-items/${this.editingItem.id}`, this.editingItem)
        .then(response => {
          // const editItem = response.data;  // 获取返回的完整数据，包括 itemId
          // console.log('Item added:', editItem);  // 调试输出
          // this.auctionItems.push(response.data);
          // this.resetNewItemForm();
        })
        .catch(error => {
          this.$message.error('Error updating item.');
          console.error('Error updating item:', error);
        })
        .finally(() => {
          this.isUpdatingItem = false;
        });
    },
    deleteItem(id) {
      axios.delete(`${BACKEND_BASE_URL}/auction-items/${id}`)
        .then(response => {
          const index = this.auctionItems.findIndex(item => item.id === id);
          this.auctionItems.splice(index, 1);
        })
        .catch(error => {
          this.$message.error('Error deleting item.');
          console.error('Error deleting item:', error);
        });
    },
  },
};
</script>

<style scoped>
.admin {
  padding: 20px;
}

.add-item,
.item-list {
  margin-bottom: 20px;
}
</style>
