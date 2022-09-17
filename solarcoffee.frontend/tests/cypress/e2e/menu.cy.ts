describe("SideMenu", () => {
  beforeEach(() => {
    cy.visit("http://localhost:8080");
  });

  it("it visits the inventory page via logo", () => {
    cy.get("#imgLogo").click();
    cy.get("#inventoryTitle").contains("Inventory Dashboard");
  });

  it("it visits the inventory page via button", () => {
    cy.get("#menuInventory").click();
    cy.get("#inventoryTitle").contains("Inventory Dashboard");
  });

  it("it visits the customers page via button", () => {
    cy.get("#menuCustomers").click();
    cy.get("#customersTitle").contains("Customers");
  });

  it("it visits the invoice page via button", () => {
    cy.get("#menuInvoice").click();
    cy.get("#invoiceTitle").contains("Invoice");
  });

  it("it visits the customers page via button", () => {
    cy.get("#menuOrders").click();
    cy.get("#ordersTitle").contains("Orders");
  });
});
