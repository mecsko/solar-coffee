<template>
  <div v-if="isTimelineBuilt">
    <component
      :is="VueApexCharts"
      height="300"
      :options="options"
      :series="series"
      type="area"
      :width="'100%'"
    ></component>
  </div>
</template>

<!-- eslint-disable @typescript-eslint/no-non-null-assertion -->
<script setup lang="ts">
  import VueApexCharts from "vue3-apexcharts";
  import { ref, computed, onBeforeMount } from "vue";
  import moment from "moment";
  import { useStore } from "@/store";
  import { IInventoryTimeline } from "@/types/InventoryGraphs";
  import { IProductInventory } from "@/types/Product";

  const store = useStore();

  const props = defineProps<{
    items: IProductInventory[];
  }>();

  const snapshotTimeline = ref<IInventoryTimeline>({ productInventorySnapshots: [], timeline: [] });
  const isTimelineBuilt = ref<boolean>(false);

  const options = computed(() => {
    return {
      dataLabels: { enabled: false },
      fill: {
        type: "gradient",
      },
      stroke: {
        curve: "smooth",
      },
      xaxis: {
        type: "categories",
        categories: snapshotTimeline.value.timeline.map((d) => {
          moment.locale("hu");
          return moment(d).utcOffset(2).format("lll");
        }),
      },
    };
  });

  const series = computed(() => {
    return snapshotTimeline.value!.productInventorySnapshots.map((snapshot) => ({
      name: props.items.find((i) => i.id == snapshot.productId)?.product.name,
      data: snapshot.quantityOnHand,
    }));
  });

  onBeforeMount(async () => {
    await store.getSnapshots().then(() => {
      snapshotTimeline.value = store.InventoryTimeline!;
      isTimelineBuilt.value = store.isTimelineBuilt;
    });
  });
</script>

<style scoped lang="scss"></style>
