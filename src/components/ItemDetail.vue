<template>
  <div class="item-detail-container">
    <el-row :gutter="20">
      <!-- 左侧：商品图片 -->
      <el-col :span="16" class="image-section">
        <el-carousel :interval="4000" arrow="always">
          <el-carousel-item v-for="image in item.images" :key="image">
            <img :src="image" alt="item.name" class="item-image" />
          </el-carousel-item>
        </el-carousel>
      </el-col>

      <!-- 右侧：商品信息和出价区域 -->
      <el-col :span="8" class="info-section">
        <h1 class="item-title">{{ item.name }}</h1>
        <p class="item-price">
          Current Bid: US ${{ item.currentBid }} <br />
          Minimum Next Bid: US ${{ minimumNextBid }}
        </p>
        <div class="bid-section">
          <el-input-number v-model="bidAmount" :min="minimumNextBid" :step="bidIncrement" label="Your Bid" />
        </div>
        <div class="button-group">
          <el-button type="primary" @click="bidNow" class="bid-now-button">Bid Now</el-button>
          <el-button type="default" @click="addToCart" class="add-to-cart-button">Add to Cart</el-button>
          <el-button type="default" @click="addToWishlist" class="add-to-watchlist-button">Add to Watchlist</el-button>
        </div>
        <p class="item-description">{{ item.description }}</p>

      </el-col>
    </el-row>

    <!-- 出价历史 -->
    <el-table :data="bids" style="width: 100%" class="bid-history">
      <el-table-column prop="user" label="User" width="150"></el-table-column>
      <el-table-column prop="amount" label="Bid Amount" width="150"></el-table-column>
      <el-table-column prop="time" label="Time"></el-table-column>
    </el-table>
    
    <!-- 商品详细信息 -->
    <div class="product-details-container">
      <div class="product-details">
        <h2>Product Details</h2>
        <p>{{ item.details }}</p>
      </div>
      <!-- 聊天功能按钮 -->
      <el-button type="primary" @click="goToChat" class="chat-button">Chat with Seller</el-button>
    </div>

    <!-- 聊天窗口 -->
    <el-dialog title="Chat with Seller" :visible.sync="chatVisible" width="50%">
      <el-input
        type="textarea"
        v-model="chatMessage"
        placeholder="Type your message here..."
        class="chat-input"
      ></el-input>
      <span slot="footer" class="dialog-footer">
        <el-button @click="chatVisible = false">Cancel</el-button>
        <el-button type="primary" @click="sendMessage">Send</el-button>
      </span>
    </el-dialog>
    
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'ItemDetail',
  data() {
    return {
      item: {
        images: [],
        name: '',
        currentBid: 0,
        startingBid: 0,
        description: '',
        condition: '',
        details: '',
        stock: 1,
      },
      quantity: 1,
      bidAmount: 0, // 用户输入的出价金额
      bidIncrement: 5, // 出价步长，最小加价金额
      bids: [],
    };
  },
  computed: {
    minimumNextBid() {
      return this.item.currentBid > 0
        ? this.item.currentBid + this.bidIncrement
        : this.item.startingBid;
    },
  },
  created() {
    this.fetchItemDetails();
    this.fetchBidHistory();
  },
  methods: {
    fetchItemDetails() {
      const itemId = this.$route.params.id;
      axios.get(`http://your-api-endpoint/auction-items/${itemId}`)
        .then(response => {
          this.item = response.data;
          this.bidAmount = this.minimumNextBid; // 设置默认出价为最小下一次出价
        })
        .catch(error => {
          console.error('Error fetching item details:', error);
        });
    },
    fetchBidHistory() {
      const itemId = this.$route.params.id;
      axios.get(`http://your-api-endpoint/auction-items/${itemId}/bids`)
        .then(response => {
          this.bids = response.data;
        })
        .catch(error => {
          console.error('Error fetching bid history:', error);
        });
    },
    bidNow() {
    if (this.bidAmount < this.minimumNextBid) {
      this.$message.error(`Your bid must be at least US $${this.minimumNextBid}`);
      return;
    }
    // 传递出价金额和数量到支付页面
    this.$router.push({
      name: 'Payment',
      params: {
        itemName: this.item.name,
        amount: this.bidAmount,
        quantity: this.quantity
      }
    });
  },
    // 添加到购物车
    addToCart() {
      // 实现添加到购物车的逻辑
      this.$message.success('Added to cart!');
    },
    // 添加到愿望清单
    addToWishlist() {
      // 实现添加到愿望清单的逻辑
      this.$message.success('Added to watchlist!');
    },
    goToChat() {
      this.$router.push('/chat');  // 跳转到聊天页面
    }
  },
};
</script>

<style scoped>
.item-detail-container {
  display: flex;
  flex-direction: column;
  padding: 40px 50px;
  background-color: #f0f2f5;
  width: 100vw;
  box-sizing: border-box;
}

.image-section {
  display: flex;
  justify-content: center;
  align-items: center;
}

.item-image {
  width: 100%;
  border-radius: 10px;
  object-fit: cover;
}

.info-section {
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.item-title {
  font-size: 1.8rem;
  font-weight: bold;
  margin-bottom: 10px;
}

.item-price {
  font-size: 1.5rem;
  color: #e74c3c;
  margin-bottom: 15px;
}

.item-condition {
  font-size: 1rem;
  margin-bottom: 20px;
}

.item-description {
  font-size: 1rem;
  color: #7f8c8d;
  margin-top: 20px;
}

.quantity-section {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
}

.stock-info {
  margin-left: 10px;
  font-size: 0.9rem;
  color: #7f8c8d;
}

.button-group {
  display: flex;
  flex-direction: column;
  align-items: stretch; /* 保证按钮宽度一致 */
  gap: 10px;
}

.bid-now-button {
  width: 100%;
  padding: 15px 20px;
  font-size: 1.2rem;
  background-color: #0066c0;
  color: white;
  border-radius: 5px;
  transition: background-color 0.3s ease;
}

.bid-now-button:hover {
  background-color: #00509d;
}

.add-to-cart-button {
  width: 100%;
  padding: 15px 20px;
  font-size: 1.2rem;
  background-color: white;
  color: #0066c0;
  border: 1px solid #0066c0;
  border-radius: 5px;
  transition: background-color 0.3s ease, color 0.3s ease;
}

.add-to-cart-button:hover {
  background-color: #f5f5f5;
  color: #00509d;
}

.add-to-watchlist-button {
  width: 100%;
  padding: 15px 20px;
  font-size: 1.2rem;
  background-color: white;
  color: #0066c0;
  border: 1px solid #0066c0;
  border-radius: 5px;
  transition: background-color 0.3s ease, color 0.3s ease;
}

.add-to-watchlist-button:hover {
  background-color: #f5f5f5;
  color: #00509d;
}

.bid-history {
  margin-top: 30px;
  border-radius: 10px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.el-button + .el-button {
  margin-left: 0;
}

/* 新增的商品详细信息样式 */
.product-details-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: 30px;
}

.product-details {
  flex: 1;
  margin-right: 20px;
}

.product-details h2 {
  font-size: 1.5rem;
  margin-bottom: 10px;
}

.product-details p {
  font-size: 1rem;
  color: #7f8c8d;
}

/* 聊天按钮样式 */
.chat-button {
  flex-shrink: 0;
  padding: 15px 20px;
  font-size: 1.2rem;
  background-color: #ff9900;
  color: white;
  border-radius: 5px;
  transition: background-color 0.3s ease;
}

.chat-button:hover {
  background-color: #e68a00;
}
</style>
