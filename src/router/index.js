import { createRouter, createWebHistory } from 'vue-router';
import Home from '../components/Home.vue';
import Login from '../components/Login.vue';
import Register from '../components/Register.vue';
import UserProfile from '../components/UserProfile.vue';
import ItemDetail from '../components/ItemDetail.vue';
import Admin from '../components/Admin.vue';
import AdminLogin from '../components/AdminLogin.vue';
import Inbox from '../components/Inbox.vue';
import Chat from '../components/Chat.vue';  // 导入 Chat 组件
import Payment from '../components/Payment.vue';
import UserManual from '../components/UserManual.vue'; // 导入 UserManual 组件
import SellItem from '../components/SellItem.vue'; // 导入 SellItem 组件
import Brand from '../components/Brand.vue';  // 导入 Brand 组件


const routes = [
  { path: '/', name: 'Home', component: Home },
  { path: '/login', name: 'Login', component: Login },
  { path: '/register', name: 'Register', component: Register },
  {
    path: '/profile',
    name: 'UserProfile',
    component: UserProfile,
    meta: { requiresAuth: true }, // 需要身份验证
  },
  { path: '/item/:id', name: 'ItemDetail', component: ItemDetail },
  { path: '/admin/login', name: 'AdminLogin', component: AdminLogin },
  {
    path: '/admin',
    name: 'Admin',
    component: Admin,
    meta: { requiresAdminAuth: true },
  },
  { path: '/inbox', name: 'Inbox', component: Inbox },
  { path: '/chat', name: 'Chat', component: Chat },
  {
    path: '/payment',
    name: 'Payment',
    component: Payment,
    props: true, // 确保参数通过 props 传递到组件
  },
  { path: '/sell', name: 'SellItem', component: SellItem }, // 添加卖出商品的路由
  { path: '/manual', name: 'UserManual', component: UserManual }, // 添加使用手册的路由
  { path: '/brand', name: 'Brand', component: Brand }, // 添加品牌介绍的路由
];


const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
  const requiresAdminAuth = to.matched.some(record => record.meta.requiresAdminAuth);

  if (requiresAuth) {
    const token = localStorage.getItem('authToken');
    if (!token) return next({ name: 'Login' });
  }

  if (requiresAdminAuth) {
    const adminToken = localStorage.getItem('adminToken');
    if (!adminToken) return next({ name: 'AdminLogin' }); // 重定向到 AdminLogin 页面
  }

  next();
});

export default router;
