def build_values_array(text):
    values_array = []
    for line in text.split("\n"):
        value_array = [x for x in line]
        if len(value_array) > 0:
            values_array.append(value_array)
    return values_array


def build_bitwise_number(values):
    return int("".join([str(x) for x in values]), 2)


def calculate_rate(values_array, greek_letter):
    parts = []
    report_size = len(values_array[0])
    for i in range(0, report_size):
        entries = list(map(lambda x: x[i], values_array))
        operand = max if greek_letter == "gamma" else min
        part = operand(set(entries), key=entries.count)
        parts.append(part)
    return build_bitwise_number(parts)


def calculate_air_rating(values_array, molecule):
    current_bit = 0
    rows = values_array.copy()
    while len(rows) > 1:
        entries = list(map(lambda x: x[current_bit], rows))
        zeroes = entries.count("0")
        ones = entries.count("1")
        if molecule == "oxygen":
            criteria = "0" if zeroes > ones else "1"
        else:
            criteria = "1" if ones < zeroes else "0"
        rows = list(filter(lambda x: x[current_bit] == criteria, rows))
        current_bit += 1
    return build_bitwise_number(rows[0])


class Report:
    def __init__(self, text):
        self.text = text
        self.values_array = build_values_array(self.text)
        self.gamma = calculate_rate(self.values_array, "gamma")
        self.epsilon = calculate_rate(self.values_array, "epsilon")
        self.oxygen = calculate_air_rating(self.values_array, "oxygen")
        self.co2 = calculate_air_rating(self.values_array, "co2")

    @property
    def power_consumption(self):
        return self.gamma * self.epsilon

    @property
    def life_support(self):
        return self.oxygen * self.co2

    def __str__(self):
        title = "Submarine Diagnostics Report:"
        return f"{title}\nPower Consumption: {self.power_consumption}\nLife Support: {self.life_support}"


with open("input.txt") as f:
    text = f.read()
report = Report(text)
print(report)
