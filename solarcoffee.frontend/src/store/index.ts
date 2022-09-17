import { ref } from "vue";
import { defineStore } from "pinia";
import { InventoryService } from "@/services/inventory-service";
import { IInventoryTimeline } from "./../types/InventoryGraphs.d";

export const useStore = defineStore("main", () => {
  const InventoryTimeline = ref<IInventoryTimeline>();
  const isTimelineBuilt = ref(false);

  async function getSnapshots() {
    const inventoryService = new InventoryService();

    const res = await inventoryService.getSnapshotHistory();

    const timeline: IInventoryTimeline = {
      productInventorySnapshots: res.productInventorySnapshots,
      timeline: res.timeline,
    };
    InventoryTimeline.value = timeline;
    isTimelineBuilt.value = true;
  }

  return { InventoryTimeline, isTimelineBuilt, getSnapshots };
});
