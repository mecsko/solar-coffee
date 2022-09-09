<template>
  <solar-modal>
    <template #header>Receive Shipment</template>
    <template #body>
      <label for="product">Product Received:</label>
      <select id="product" v-model="selectedProduct" class="shipmentItems">
        <option disabled value="null">Please select one</option>
        <option v-for="item in props.inventory" :key="item.product.id" :value="item.product">
          {{ item.product.name }}
        </option>
      </select>
      <label for="qtyReceived">Quantity Received</label>
      <input id="qtyReceived" v-model="qtyReceived" type="number" />
    </template>
    <template #footer>
      <solar-button aria-label="Save new shipment" type="button" @click="save">
        Save Received Shipment
      </solar-button>
      <solar-button aria-label="Close modal" type="button" @click="close">Close</solar-button>
    </template>
  </solar-modal>
</template>

<script setup lang="ts">
  import { ref } from "vue";
  import solarModal from "@/components/modals/SolarModal.vue";
  import solarButton from "@/components/SolarButton.vue";
  import { IProduct, IProductInventory } from "@/types/Product";
  import { IShipment } from "@/types/Shipment";

  const props = defineProps({
    inventory: {
      type: Array as () => IProductInventory[],
      required: true,
    },
  });

  const selectedProduct = ref<IProduct | null>(null);

  const qtyReceived = ref(0);

  const emit = defineEmits<{
    (event: "close"): void;
    (event: "save:shipment", shipment: IShipment): void;
  }>();

  function close() {
    emit("close");
  }
  function save() {
    if (selectedProduct.value) {
      const shipment: IShipment = {
        productId: selectedProduct.value.id,
        adjustment: qtyReceived.value,
      };
      emit("save:shipment", shipment);
    }
  }
</script>

<style scoped lang="scss"></style>
