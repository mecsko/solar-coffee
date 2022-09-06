<template>
  <div class="btn-link">
    <button
      :class="['solar-button', { 'full-width': props.isFullWidth }]"
      type="button"
      @click="onClick"
    >
      <slot></slot>
    </button>
  </div>
</template>
<script setup lang="ts">
  const props = defineProps({
    isFullWidth: {
      required: false,
      default: false,
      type: Boolean,
    },
  });

  const emit = defineEmits(["button:click"]);
  const onClick = () => emit("button:click");
</script>
<style scoped lang="scss">
  @import "@/scss/global.scss";

  .full-width {
    display: block;
    width: 100%;
  }

  .solar-button {
    background: lighten($solar-blue, 10%);
    color: white;
    padding: 0.8rem;
    transition: background-color 0.5s;
    margin: 0.3rem 0.2rem;
    display: inline-block;
    cursor: pointer;
    font-size: 1rem;
    min-width: 100px;
    border: none;
    border-bottom: 2px solid darken($solar-blue, 20%);
    border-radius: 3px;

    &:hover {
      background: lighten($solar-blue, 20%);
      transition: background-color 0.5s;
    }

    &:disabled {
      background: lighten($solar-blue, 15%);
      border-bottom: 2px solid lighten($solar-blue, 20%);
    }

    &:active {
      background: $solar-yellow;
      border-bottom: 2px solid lighten($solar-yellow, 20%);
    }
  }
</style>
