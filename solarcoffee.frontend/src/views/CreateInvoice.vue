<template>
  <div>
    <h1 id="invoiceTitle">Create Invoice</h1>
    <br />

    <div v-if="invoiceStep === 1" class="invoice-step">
      <h1>Step 1: Select Customer</h1>
      <div v-if="customers.length" class="invoice-step-detail">
        <label for="customer">Customer:</label>
        <select id="customer" v-model="selectedCustomerId" class="invoice-customers">
          <option disabled value="null">Please select a Customer</option>
          <option v-for="customer in customers" :key="customer.id" :value="customer.id">
            {{ customer.firstName + " " + customer.lastName }}
          </option>
        </select>
      </div>
    </div>
    <div v-if="invoiceStep === 2" class="invoice-step">
      <h1>Step 2: Create Order</h1>
      <div v-if="inventory.length" class="invoice-step-detail">
        <label for="product">Product:</label>
        <select id="product" v-model="newItem.product" class="invoiceLineItem">
          <option disabled value="null">Please select a Product</option>
          <option v-for="i in inventory" :key="i.product.id" :value="i.product">
            {{ i.product.name }} || Qty.: {{ i.quantityOnHand }}
          </option>
        </select>
        <label for="quantity">Quantity:</label>
        <input id="quantity" v-model="newItem.quantity" min="0" type="number" />
      </div>

      <div class="invoice-item-actions">
        <solar-button
          :is-disabled="!newItem.product || !newItem.quantity"
          @button:click="addLineItem"
        >
          Add Line Item
        </solar-button>
        <solar-button :is-disabled="!lineItems.length" @button:click="finalizeOrder">
          Finalize Order
        </solar-button>
      </div>

      <div v-if="lineItems.length" class="invoice-order-list">
        <div class="runningTotal">
          <h3>Running total:</h3>
          {{ runningTotal }}
        </div>
        <hr />
        <table class="table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Description</th>
              <th>Qty.</th>
              <th>Price</th>
              <th>Subtotal</th>
            </tr>
          </thead>
          <tr v-for="lineItem in lineItems" :key="`index_${lineItem.product?.id}`">
            <td>{{ lineItem.product?.name }}</td>
            <td>{{ lineItem.product?.description }}</td>
            <td>{{ lineItem.quantity }}</td>
            <td>{{ lineItem.product?.price }}</td>
            <td>{{ price(lineItem.product!.price * lineItem.quantity) }}</td>
          </tr>
        </table>
      </div>
    </div>
    <div v-if="invoiceStep === 3" class="invoice-step">
      <h2>Step 3: Review and Submit</h2>
      <solar-button @button:click="submitInvoice">Submit Invoice</solar-button>
      <hr />

      <div id="invoice" ref="invoice" class="invoice-step-detail">
        <div class="invoice-logo">
          <img id="imgLogo" alt="Solar Coffee Logo" src="../assets/logo.png" />
          <h3>1333 Solar Lane</h3>
          <h3>San Angeles, CA 90002</h3>
          <h3>USA</h3>

          <div v-if="lineItems.length" class="invoice-order-list">
            <div class="invoice-header">
              <h3>Invoice: {{ date(new Date()) }}</h3>
              <h3>
                Customer: {{ selectedCustomer?.firstName + " " + selectedCustomer?.lastName }}
              </h3>
              <h3>Address: {{ selectedCustomer?.primaryAddress.addressLine1 }}</h3>
              <h3 v-if="selectedCustomer?.primaryAddress.addressLine2">
                {{ selectedCustomer?.primaryAddress.addressLine2 }}
              </h3>
              <h3>
                {{ selectedCustomer?.primaryAddress.city }},
                {{ selectedCustomer?.primaryAddress.state }},
                {{ selectedCustomer?.primaryAddress.postalCode }}
              </h3>
              <h3>{{ selectedCustomer?.primaryAddress.country }}</h3>
            </div>
            <table class="table">
              <thead>
                <tr>
                  <th>Product</th>
                  <th>Description</th>
                  <th>Qty.</th>
                  <th>Price</th>
                  <th>Subtotal</th>
                </tr>
              </thead>
              <tr v-for="lineItem in lineItems" :key="`index_${lineItem.product?.id}`">
                <td>{{ lineItem.product?.name }}</td>
                <td>{{ lineItem.product?.description }}</td>
                <td>{{ lineItem.quantity }}</td>
                <td>{{ lineItem.product?.price }}</td>
                <td>{{ price(lineItem.product!.price * lineItem.quantity) }}</td>
              </tr>
              <tr>
                <th colspan="4"></th>
                <th>Grand Total</th>
              </tr>
              <tfoot>
                <tr>
                  <td class="due" colspan="4">Balance due upon receipt:</td>
                  <td class="price-final">{{ runningTotal }}</td>
                </tr>
              </tfoot>
            </table>
          </div>
        </div>
      </div>
    </div>
    <hr />
    <div class="invoice-steps-actions">
      <solar-button :is-disabled="!canGoPrev" @button:click="prev">Previous</solar-button>
      <solar-button :is-disabled="!canGoNext" @button:click="next">Next</solar-button>
      <solar-button @button:click="startOver">Start Over</solar-button>
    </div>
  </div>
</template>

<!-- eslint-disable @typescript-eslint/no-non-null-assertion -->
<script setup lang="ts">
  import { computed, onBeforeMount, ref } from "vue";
  import { useRouter } from "vue-router";
  import jspdf from "jspdf";
  import html2camvas from "html2canvas";
  import solarButton from "@/components/SolarButton.vue";
  import { CustomerService } from "@/services/customer-service";
  import { InventoryService } from "@/services/inventory-service";
  import { InvoiceSerice } from "@/services/invoice-service";
  import { date, price } from "@/utils/humanize";
  import { ICustomer } from "@/types/Customer";
  import { IInvoice, ILineItem } from "@/types/Invoice";
  import { IProductInventory } from "@/types/Product";

  const router = useRouter();
  const customerService = new CustomerService();
  const inventoryService = new InventoryService();
  const invoiceService = new InvoiceSerice();

  const invoiceStep = ref(1);
  const invoice = ref<IInvoice>({
    createdOn: new Date(),
    customerId: 0,
    lineItems: [],
    updatedOn: new Date(),
  });
  const customers = ref<ICustomer[]>([]);
  const selectedCustomerId = ref<number | null>(null);
  const inventory = ref<IProductInventory[]>([]);
  const lineItems = ref<ILineItem[]>([]);
  const newItem = ref<ILineItem>({ product: null, quantity: 0 });

  function prev() {
    if (invoiceStep.value === 1) {
      return;
    }
    invoiceStep.value -= 1;
  }

  function next() {
    if (invoiceStep.value === 3) {
      return;
    }
    invoiceStep.value += 1;
  }

  function startOver() {
    invoice.value = { customerId: 0, lineItems: [] };
    invoiceStep.value = 1;
    selectedCustomerId.value = null;
  }

  const canGoPrev = computed(() => {
    if (invoiceStep.value === 1) {
      return false;
    } else if (invoiceStep.value === 2) {
      return true;
    } else if (invoiceStep.value === 3) {
      return true;
    }
    return false;
  });

  const canGoNext = computed(() => {
    if (invoiceStep.value === 1) {
      return selectedCustomerId.value != null;
    } else if (invoiceStep.value === 2) {
      return true;
    } else if (invoiceStep.value === 3) {
      return false;
    }
    return false;
  });

  const runningTotal = computed(() => {
    return price(lineItems.value.reduce((a, b) => a + b.product!.price * b.quantity, 0));
  });

  const selectedCustomer = computed(() => {
    return customers.value.find((c) => c.id! == selectedCustomerId.value);
  });

  function downloadPdf() {
    const pdf = new jspdf("p", "pt", "a4", true);
    const invoiceElem = document.getElementById("invoice")!;
    const width = invoiceElem?.clientWidth;
    const height = invoiceElem?.clientHeight;
    html2camvas(invoiceElem).then((canvas) => {
      const image = canvas.toDataURL("image/png");
      pdf.addImage(image, "PNG", 0, 0, width! * 0.55, height! * 0.55);
      pdf.save("invoice");
    });
  }

  async function submitInvoice() {
    invoice.value = {
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      customerId: selectedCustomerId.value!,
      lineItems: lineItems.value,
    };

    await invoiceService.makeNewInvoice(invoice.value);

    downloadPdf();
    router.push("/orders");
  }

  function addLineItem() {
    const newItemTemp: ILineItem = newItem.value;

    const existingItems = lineItems.value.map((item) => item.product?.id);

    if (existingItems.includes(newItemTemp.product?.id)) {
      const lineItem = lineItems.value.find(
        (item) => item.product?.id === newItem.value.product?.id
      );
      lineItem!.quantity = lineItem!.quantity += newItemTemp.quantity;
    } else {
      lineItems.value.push(newItem.value);
    }

    newItem.value = { product: null, quantity: 0 };
  }

  function finalizeOrder() {
    invoiceStep.value = 3;
  }

  async function initialize() {
    customers.value = (await customerService.getCustomers()).data;
    inventory.value = await inventoryService.getInventory();
  }

  onBeforeMount(async () => {
    await initialize();
  });
</script>

<style scoped lang="scss">
  @import "@/scss/global.scss";
  .invoice-steps-actions {
    display: flex;
    width: 100%;
  }
  // .invoice-step {
  // }

  .invoice-step-detail {
    margin: 1.2rem;
  }

  .invoice-order-list {
    margin-top: 1.2rem;
    padding: 0.8rem;

    .line-item {
      display: flex;
      border-bottom: 1px dashed #ccc;
      padding: 0.8rem;
    }

    .item-col {
      flex-grow: 1;
    }
  }

  .invoice-item-actions {
    display: flex;
  }

  .price-pre-tax {
    font-weight: bold;
  }

  .price-final {
    font-weight: bold;
    color: $solar-green;
  }

  .due {
    font-weight: bold;
  }

  .invoice-header {
    width: 100%;
    margin-bottom: 1.2rem;
    text-align: center;
  }

  .invoice-logo {
    padding-top: 1.4rem;
    text-align: center;

    img {
      width: 280px;
    }
  }

  h3 {
    text-align: center;
  }
</style>
