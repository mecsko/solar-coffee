import { mount } from "@vue/test-utils";
import SideMenu from "../../src/components/SideMenu.vue";
import SolarButton from "../../src/components/SolarButton.vue";

describe("SideMenu.vue", () => {
  const wrapper = mount(SideMenu, {
    global: {
      components: { SolarButton },
    },
  });
  it("renders correct number of button links", () => {
    expect(wrapper.findAll(".solar-button").length).toBe(4);
  });
});
