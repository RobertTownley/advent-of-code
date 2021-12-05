use std::fs;

const IS_PART_ONE: bool = false;

fn main() {
    let data = fs::read_to_string("input.txt").unwrap();
    let mut grid = Grid::new(data);
    grid.pass_lines();
    let answer = grid.multiple_cross_coord_count();
    println!("There are {} dangerous coordinates", answer);
}

struct Grid {
    coords: Vec<Coord>,
    vent_lines: Vec<VentLine>,
}

impl Grid {
    fn new(text: String) -> Grid {
        // Build vent lines
        let text_lines = text.split("\n").filter(|t| t.len() > 0);
        let vent_lines = text_lines.map(|text_line| {
            let parts: Vec<&str> = text_line.split(" -> ").collect();
            VentLine::new(parts[0].to_string(), parts[1].to_string())
        });

        // Build coordinates of grid to pass over
        let all_coords = vent_lines.clone().fold(Vec::new(), |mut acc, vent_line| {
            acc.push(vent_line.start);
            acc.push(vent_line.end);
            acc
        });
        let x_values = all_coords.iter().map(|c| c.x);
        let y_values = all_coords.iter().map(|c| c.y);
        let smallest_x = x_values.clone().min().unwrap();
        let largest_x = x_values.clone().max().unwrap();
        let smallest_y = y_values.clone().min().unwrap();
        let largest_y = y_values.clone().max().unwrap();

        let mut coords = Vec::new();
        for x in smallest_x..largest_x {
            for y in smallest_y..largest_y {
                coords.push(Coord {
                    times_crossed: 0,
                    x,
                    y,
                });
            }
        }

        Grid {
            coords,
            vent_lines: vent_lines.collect(),
        }
    }

    fn pass_lines(&mut self) {
        for coord in &mut self.coords {
            for line in &self.vent_lines {
                if line.contains_coord(coord) {
                    /*
                    println!(
                        "{},{} intersects {},{}->{},{}",
                        coord.x, coord.y, line.start.x, line.start.y, line.end.x, line.end.y
                    );
                    */
                    coord.increment_cross_count();
                    continue;
                }
            }
        }
    }

    fn multiple_cross_coord_count(&self) -> i64 {
        let mut answer = 0;
        for coord in &self.coords {
            if coord.times_crossed > 1 {
                answer += 1;
            }
        }
        answer
    }
}

struct VentLine {
    start: Coord,
    end: Coord,
}
impl VentLine {
    fn new(start_text: String, end_text: String) -> VentLine {
        VentLine {
            start: Coord::new(start_text),
            end: Coord::new(end_text),
        }
    }

    fn contains_coord(&self, coord: &Coord) -> bool {
        let c = coord;
        let a = &self.start;
        let b = &self.end;
        if IS_PART_ONE {
            if a.x != b.x && a.y != b.y {
                return false;
            }
        };
        let cross_product = (c.y - a.y) * (b.x - a.x) - (c.x - a.x) * (b.y - a.y);

        if cross_product != 0 {
            return false;
        }

        let dot_product = (c.x - a.x) * (b.x - a.x) + (c.y - a.y) * (b.y - a.y);
        if dot_product < 0 {
            return false;
        };

        let squares = (b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y);
        if dot_product > squares {
            return false;
        }
        return true;
    }
}

struct Coord {
    times_crossed: i64,
    x: i64,
    y: i64,
}

impl Coord {
    fn new(text: String) -> Coord {
        let parts: Vec<&str> = text.split(",").collect();
        Coord {
            times_crossed: 0,
            x: parts[0].parse::<i64>().unwrap(),
            y: parts[1].parse::<i64>().unwrap(),
        }
    }

    fn increment_cross_count(&mut self) {
        self.times_crossed += 1;
    }
}
