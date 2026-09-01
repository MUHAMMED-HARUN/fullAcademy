import CityView from "@/views/Address/CityView.vue";
import CountryView from "@/views/Address/CountryView.vue";
import DistrictView from "@/views/Address/DistrictView.vue";
import NeighborhoodView from "@/views/Address/NeighborhoodView.vue";

export const AddressLinks = [
  {
    path: "/country",
    name: "Country",
    component: CountryView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/city",
    name: "City",
    component: CityView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/district",
    name: "District",
    component: DistrictView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/neighborhood",
    name: "Neighborhood",
    component: NeighborhoodView,
    meta: {
      requiresAuth: true,
    },
  },
];
