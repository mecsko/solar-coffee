<template>
  <div class="side-menu-container">
    <router-link to="/"><img alt="logo" src="../assets/logo.png" /></router-link>
    <h1>Management Portal</h1>
    <solar-button id="menuInventory" is-full-width @button:click="goToRoute('/inventory')">
      Inventory
    </solar-button>
    <solar-button id="menuCustomers" is-full-width @button:click="goToRoute('/customers')">
      Manage Customers
    </solar-button>
    <solar-button id="menuInvoice" is-full-width @button:click="goToRoute('/invoice/new')">
      New Invoice
    </solar-button>
    <solar-button id="menuOrders" is-full-width @button:click="goToRoute('/orders')">
      Orders
    </solar-button>
  </div>
</template>
<script setup lang="ts">
  import solarButton from "./SolarButton.vue";
  import { useRouter } from "vue-router";

  const router = useRouter();

  function goToRoute(route: string) {
    router.push(route);
  }
</script>
<style scoped lang="scss">
  @import "@/scss/global.scss";
  .side-menu-container {
    background-color: #fcfcfc;
    height: 100vh;
    width: $menu-width;
    display: flex;
    flex-direction: column;
    padding: 0.8rem;
    box-shadow: 0 1px 1px rgba($color: #000000, $alpha: 0.12),
      0 1px 1px rgba($color: #000000, $alpha: 0.24);
    box-sizing: border-box;
  }

  #imgLogo {
    width: 100%;
  }

  h1 {
    font-size: 1.2rem;
    margin: 1rem 0;
  }
</style>
