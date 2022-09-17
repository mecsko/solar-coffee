<template>
  <div class="app">
    <div v-if="!closed" class="app-menu">
      <side-menu @close="close" />
    </div>
    <button v-else class="close-button" @click="close">&#10145;</button>
    <!-- width: closed ? '100%' : '90%',  10145 11013-->
    <div :class="!closed ? 'app-content' : 'app-content-full'">
      <router-view />
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref } from "vue";
  import sideMenu from "@/components/SideMenu.vue";

  const closed = ref(false);

  function close() {
    closed.value = !closed.value;
  }
</script>

<style lang="scss">
  @import "@/scss/global.scss";
  #app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
    display: flex;
  }

  .app-menu {
    position: fixed;
  }

  .app-content {
    padding: 1.2rem;
    width: 90%;
    margin: 1rem auto 1rem $menu-width;
  }

  .app-content-full {
    padding: 1.2rem;
    width: auto;
    margin: auto;
  }

  .close-button {
    font-size: 25px;
    border: none;
    background-color: white;
  }

  * {
    margin: 0;
    text-align: start;
  }

  a {
    text-decoration: none;
  }

  hr {
    border: 1px solid #eee;
    margin: 1rem 0;
  }
</style>
