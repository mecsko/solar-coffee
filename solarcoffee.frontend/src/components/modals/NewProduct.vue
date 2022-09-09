<template>
  <solar-modal>
    <template #header>Add New Product</template>
    <template #body>
      <ul class="newProduct">
        <li class="checkboxList">
          <label for="isTaxable">Is this taxable?</label>
          <input id="isTaxable" v-model="newProduct.isTaxable" type="checkbox" />
        </li>
        <li>
          <label for="productName">Name</label>
          <input id="productName" v-model="newProduct.name" type="text" />
        </li>
        <li>
          <label for="productDesc">Description</label>
          <input id="productDesc" v-model="newProduct.description" type="text" />
        </li>
        <li>
          <label for="productPrice">Price (USD)</label>
          <input id="productPrice" v-model="newProduct.price" type="number" />
        </li>
      </ul>
    </template>
    <template #footer>
      <solar-button aria-label="Save new item" type="button" @click="save">
        Save Product
      </solar-button>
      <solar-button aria-label="Close modal" type="button" @click="close">Close</solar-button>
    </template>
  </solar-modal>
</template>

<script setup lang="ts">
  import { reactive } from "vue";
  import solarModal from "@/components/modals/SolarModal.vue";
  import solarButton from "@/components/SolarButton.vue";
  import { IProduct } from "@/types/Product";

  const newProduct = reactive<IProduct>({
    createdOn: new Date(),
    updatedOn: new Date(),
    id: 0,
    description: "",
    isTaxable: false,
    name: "",
    price: 0,
    isArchived: false,
  });

  const emit = defineEmits<{
    (event: "close"): void;
    (event: "save:product", product: IProduct): void;
  }>();

  function close() {
    emit("close");
  }
  function save() {
    emit("save:product", newProduct);
  }
</script>

<style scoped lang="scss">
  .newProduct {
    list-style: none;
    padding: 0;
    margin: 0;
  }

  input {
    width: 100%;
    height: 1.8rem;
    margin-bottom: 1.2rem;
    font-size: 1.1rem;
    line-height: 1.3rem;
    padding: 0.2rem;
    color: #555;
  }

  .checkboxList {
    width: 100%;

    input {
      width: 20px;
      height: 1.8rem;
      margin-bottom: 1.2rem;
      font-size: 1.1rem;
      line-height: 1.3rem;
      padding: 0.2rem;
      color: #555;
    }
  }

  label {
    font-weight: bold;
    display: block;
    margin-bottom: 0.3rem;
  }
</style>
