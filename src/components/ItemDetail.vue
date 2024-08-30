<template>
    <div class="item-detail">
      <h1>{{ item.name }}</h1>
      <img :src="item.imageUrl" alt="item.name" class="item-image" />
      <p>Starting Bid: ${{ item.startingBid }}</p>
      <p>{{ item.description }}</p>
      <div class="bid-section">
        <input type="number" v-model="bidAmount" placeholder="Enter your bid" />
        <button @click="placeBid">Place a Bid</button>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: 'ItemDetail',
    data() {
      return {
        item: {},
        bidAmount: 0,
      };
    },
    created() {
      this.fetchItemDetails();
    },
    methods: {
      fetchItemDetails() {
        const itemId = this.$route.params.id;
        axios.get(`http://your-api-endpoint/auction-items/${itemId}`)
          .then(response => {
            this.item = response.data;
          })
          .catch(error => {
            console.error('Error fetching item details:', error);
          });
      },
      placeBid() {
        const userId = localStorage.getItem('userId');
        axios.post(`http://your-api-endpoint/auction-items/${this.item.id}/bids`, {
          userId: userId,
          amount: this.bidAmount,
        })
        .then(response => {
          console.log('Bid placed successfully:', response.data);
          // 处理竞价成功的逻辑
        })
        .catch(error => {
          console.error('Error placing bid:', error);
        });
      },
    },
  };
  </script>
  
  <style scoped>
  .item-detail {
    padding: 20px;
  }
  .item-image {
    width: 100%;
    height: auto;
  }
  .bid-section {
    margin-top: 20px;
  }
  </style>
  