import { shallowMount } from "@vue/test-utils";
import SolarButton from "../../src/components/SolarButton.vue";

describe("SolarButton.vue", () => {
  it("displays text in default slot position", () => {
    const wrapper = shallowMount(SolarButton, {
      props: {},
      slots: { default: "click here" },
    });

    expect(wrapper.find("button").text()).toBe("click here");
  });

  it("has underlying disabled button when disabled=true passed as prop", () => {
    const wrapper = shallowMount(SolarButton, {
      props: { isDisabled: true },
      slots: { default: "foo" },
    });

    // console.log("disabled", wrapper.find("button:disabled"));

    expect(wrapper.find("button:disabled").exists());
  });

  it("has underlying disabled button when disabled=false passed as prop", () => {
    const wrapper = shallowMount(SolarButton, {
      props: { isDisabled: false },
      slots: { default: "foo" },
    });

    // console.log("not disabled", wrapper.find("button:disabled"));

    expect(wrapper.find("button:disabled").exists()).not.toBe(true);
  });
});
