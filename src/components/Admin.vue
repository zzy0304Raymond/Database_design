<template>
  <div class="admin">
    <h1>Admin Panel</h1>
    <div class="add-item">
      <h2>Add New Item</h2>
      <el-form @submit.prevent="addItem" :model="newItem" ref="newItemForm">
        <el-form-item label="Name" :rules="[{ required: true, message: 'Please input the item name', trigger: 'blur' }]">
          <el-input v-model="newItem.name"></el-input>
        </el-form-item>
        <el-form-item label="Starting Bid" :rules="[{ required: true, message: 'Please input the starting bid', trigger: 'blur' }]">
          <el-input type="number" v-model="newItem.startingBid"></el-input>
        </el-form-item>
        <el-form-item label="Category" :rules="[{ required: true, message: 'Please input the category', trigger: 'blur' }]">
          <el-input v-model="newItem.category"></el-input>
        </el-form-item>
        <el-form-item label="End Time" :rules="[{ required: true, message: 'Please select the end time', trigger: 'change' }]">
          <el-date-picker v-model="newItem.endTime" type="datetime" placeholder="Select date and time"></el-date-picker>
        </el-form-item>
        <el-form-item label="Image URL" :rules="[{ required: true, message: 'Please input the image URL', trigger: 'blur' }]">
          <el-input v-model="newItem.imageUrl"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" :loading="isAddingItem" @click="submitNewItemForm">Add Item</el-button>
          <el-button @click="resetNewItemForm">Reset</el-button> <!-- 添加重置按钮 -->
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
    <el-dialog v-if="editingItem" title="Edit Item" :visible.sync="editItemDialogVisible">
      <el-form :model="editingItem" ref="editItemForm">
        <el-form-item label="Name" :rules="[{ required: true, message: 'Please input the item name', trigger: 'blur' }]">
          <el-input v-model="editingItem.name"></el-input>
        </el-form-item>
        <el-form-item label="Starting Bid" :rules="[{ required: true, message: 'Please input the starting bid', trigger: 'blur' }]">
          <el-input type="number" v-model="editingItem.startingBid"></el-input>
        </el-form-item>
        <el-form-item label="Category" :rules="[{ required: true, message: 'Please input the category', trigger: 'blur' }]">
          <el-input v-model="editingItem.category"></el-input>
        </el-form-item>
        <el-form-item label="End Time" :rules="[{ required: true, message: 'Please select the end time', trigger: 'change' }]">
          <el-date-picker v-model="editingItem.endTime" type="datetime" placeholder="Select date and time"></el-date-picker>
        </el-form-item>
        <el-form-item label="Image URL" :rules="[{ required: true, message: 'Please input the image URL', trigger: 'blur' }]">
          <el-input v-model="editingItem.imageUrl"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" :loading="isUpdatingItem" @click="submitEditItemForm">Update Item</el-button>
        </el-form-item>
      </el-form>
    </el-dialog>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Admin',
  data() {
    return {
      newItem: {
        name: '',
        startingBid: 0,
        category: '',
        endTime: '',
        imageUrl: '',
      },
      auctionItems: [],
      editingItem: null,
      isAddingItem: false,
      isUpdatingItem: false,
      editItemDialogVisible: false,
    };
  },
  created() {
    this.fetchAuctionItems();
  },
  methods: {
    fetchAuctionItems() {
      axios.get('http://localhost:5033/auction-items')
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
    addItem() {
      this.isAddingItem = true;
      axios.post('http://localhost:5033/auction-items', this.newItem)
        .then(response => {
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
      this.$refs.newItemForm.resetFields();
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
      axios.put(`http://localhost:5033/auction-items/${this.editingItem.id}`, this.editingItem)
        .then(response => {
          const index = this.auctionItems.findIndex(item => item.id === this.editingItem.id);
          this.$set(this.auctionItems, index, response.data);
          this.editItemDialogVisible = false;
          this.editingItem = null;
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
      axios.delete(`http://localhost:5033/auction-items/${id}`)
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
