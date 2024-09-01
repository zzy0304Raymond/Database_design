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
    meta: { requiresAuth: true },
  },
  { path: '/inbox', name: 'Inbox', component: Inbox },
  { path: '/chat', name: 'Chat', component: Chat },
  {
    path: '/payment',
    name: 'Payment',
    component: Payment,
    props: true, // 确保参数通过 props 传递到组件
  },
];


const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    const token = localStorage.getItem('authToken');
    if (token) {
      next();
    } else {
      next({ name: 'Login' });
    }
  } else {
    next();
  }
});

router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    const adminToken = localStorage.getItem('adminToken');
    if (adminToken) {
      next();
    } else {
      next({ name: 'AdminLogin' });
    }
  } else {
    next();
  }
});

export default router;
