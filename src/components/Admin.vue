<template>
    <div class="admin">
      <h1>Admin Panel</h1>
      <div class="add-item">
        <h2>Add New Item</h2>
        <form @submit.prevent="addItem">
          <div>
            <label for="name">Name:</label>
            <input type="text" v-model="newItem.name" required />
          </div>
          <div>
            <label for="startingBid">Starting Bid:</label>
            <input type="number" v-model="newItem.startingBid" required />
          </div>
          <div>
            <label for="category">Category:</label>
            <input type="text" v-model="newItem.category" required />
          </div>
          <div>
            <label for="endTime">End Time:</label>
            <input type="datetime-local" v-model="newItem.endTime" required />
          </div>
          <div>
            <label for="imageUrl">Image URL:</label>
            <input type="text" v-model="newItem.imageUrl" required />
          </div>
          <button type="submit">Add Item</button>
        </form>
      </div>
      <div class="item-list">
        <h2>Item List</h2>
        <ul>
          <li v-for="item in auctionItems" :key="item.id">
            {{ item.name }} - ${{ item.startingBid }}
            <button @click="editItem(item.id)">Edit</button>
            <button @click="deleteItem(item.id)">Delete</button>
          </li>
        </ul>
      </div>
      <div v-if="editingItem" class="edit-item">
        <h2>Edit Item</h2>
        <form @submit.prevent="updateItem">
          <div>
            <label for="name">Name:</label>
            <input type="text" v-model="editingItem.name" required />
          </div>
          <div>
            <label for="startingBid">Starting Bid:</label>
            <input type="number" v-model="editingItem.startingBid" required />
          </div>
          <div>
            <label for="category">Category:</label>
            <input type="text" v-model="editingItem.category" required />
          </div>
          <div>
            <label for="endTime">End Time:</label>
            <input type="datetime-local" v-model="editingItem.endTime" required />
          </div>
          <div>
            <label for="imageUrl">Image URL:</label>
            <input type="text" v-model="editingItem.imageUrl" required />
          </div>
          <button type="submit">Update Item</button>
        </form>
      </div>
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
      };
    },
    created() {
      this.fetchAuctionItems();
    },
    methods: {
      fetchAuctionItems() {
        axios.get('http://your-api-endpoint/auction-items')
          .then(response => {
            this.auctionItems = response.data;
          })
          .catch(error => {
            console.error('Error fetching auction items:', error);
          });
      },
      addItem() {
        axios.post('http://your-api-endpoint/auction-items', this.newItem)
          .then(response => {
            this.auctionItems.push(response.data);
            this.newItem = { name: '', startingBid: 0, category: '', endTime: '', imageUrl: '' };
          })
          .catch(error => {
            console.error('Error adding item:', error);
          });
      },
      editItem(id) {
        const item = this.auctionItems.find(item => item.id === id);
        this.editingItem = { ...item };
      },
      updateItem() {
        axios.put(`http://your-api-endpoint/auction-items/${this.editingItem.id}`, this.editingItem)
          .then(response => {
            const index = this.auctionItems.findIndex(item => item.id === this.editingItem.id);
            this.$set(this.auctionItems, index, response.data);
            this.editingItem = null;
          })
          .catch(error => {
            console.error('Error updating item:', error);
          });
      },
      deleteItem(id) {
        axios.delete(`http://your-api-endpoint/auction-items/${id}`)
          .then(response => {
            const index = this.auctionItems.findIndex(item => item.id === id);
            this.auctionItems.splice(index, 1);
          })
          .catch(error => {
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
  .item-list,
  .edit-item {
    margin-bottom: 20px;
  }
  </style>
  